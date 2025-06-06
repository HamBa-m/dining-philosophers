package com.example.DiningPhilosophers

import akka.actor.{Actor, ActorSelection, Cancellable}
import akka.util.Timeout
import akka.pattern.ask
import scala.concurrent.duration._
import scala.util.{Success, Failure}

object PhilosopherMessages {
  case object StartThinking
  case object FinishedThinking
  case object TryToEat
  case object StartEating
  case object FinishedEating
  case object RetryLater
}

/**
 * Philosopher Entity - Designed to engage with remote fork resource units
 * Supports both a standard deadlock-prone approach and a priority-based deadlock avoidance technique
 * Under the priority-based method, entities secure the lower-priority fork first to avoid circular dependencies
 */
class Philosopher(id: Int, leftFork: ActorSelection, leftPriority: Int, rightFork: ActorSelection, rightPriority: Int, useHierarchySolution: Boolean) extends Actor {
  
  import PhilosopherMessages._
  import ForkMessages._
  import context.dispatcher
  
  // Timing configurations
  implicit val timeout: Timeout = Timeout(5.seconds)
  private def thinkingTime: FiniteDuration = 3000.millis
  private def eatingTime: FiniteDuration = 2000.millis
  private def retryDelay: FiniteDuration = 1000.millis
  
  // Set fork order based on selected strategy
  private val (firstFork, secondFork) =
    if (useHierarchySolution) {
      if (leftPriority < rightPriority) (leftFork, rightFork)
      else (rightFork, leftFork)
    } else {
      (leftFork, rightFork) // Default to left-first without hierarchy
    }
  
  // Track fork possession
  private var hasFirstFork = false
  private var hasSecondFork = false
  private var currentTask: Option[Cancellable] = None
  
  override def preStart(): Unit = {
    val method = if (useHierarchySolution) "Priority-Based Hierarchy" else "standard first-left"
    println(s"[Philosopher-$id] Activated with method: $method")
  }
  
  override def postStop(): Unit = {
    currentTask.foreach(_.cancel())
    if (hasFirstFork) firstFork ! Release(id)
    if (hasSecondFork) secondFork ! Release(id)
  }
  
  def receive: Receive = {
    case StartThinking =>
      println(s"[Philosopher-$id] Beginning contemplation period...")
      scheduleMessage(thinkingTime, FinishedThinking)
      
    case FinishedThinking =>
      println(s"[Philosopher-$id] Done contemplating, feeling peckish!")
      self ! TryToEat
      
    case TryToEat =>
      attemptToSecureForks()
      
    case StartEating =>
      println(s"[Philosopher-$id] Obtained both forks, starting meal!")
      scheduleMessage(eatingTime, FinishedEating)
      
    case FinishedEating =>
      println(s"[Philosopher-$id] Finished meal, relinquishing forks")
      releaseForks()
      self ! StartThinking
      
    case RetryLater =>
      println(s"[Philosopher-$id] Attempting to secure forks again...")
      self ! TryToEat
  }
  
  private def attemptToSecureForks(): Unit = {
    if (!hasFirstFork) {
      println(s"[Philosopher-$id] Trying to pick up first fork...")
      val firstFuture = (firstFork ? TryAcquire(id))
      firstFuture.onComplete {
        case Success(AcquireSuccess) =>
          hasFirstFork = true
          println(s"[Philosopher-$id] First fork secured")
          self ! TryToEat
          
        case Success(AcquireFailed) =>
          println(s"[Philosopher-$id] First fork unavailable, will retry soon")
          scheduleMessage(retryDelay, RetryLater)

        case Success(other) =>
          println(s"[Philosopher-$id] Unexpected reply for first fork: $other")
          scheduleMessage(retryDelay, RetryLater)
          
        case Failure(exception) =>
          println(s"[Philosopher-$id] Error securing first fork: ${exception.getMessage}")
          scheduleMessage(retryDelay, RetryLater)
      }
      
    } else if (!hasSecondFork) {
      println(s"[Philosopher-$id] Trying to pick up second fork...")
      val secondFuture = (secondFork ? TryAcquire(id))
      secondFuture.onComplete {
        case Success(AcquireSuccess) =>
          hasSecondFork = true
          println(s"[Philosopher-$id] Second fork secured")
          self ! StartEating
          
        case Success(AcquireFailed) =>
          println(s"[Philosopher-$id] Second fork unavailable, will retry soon")
          scheduleMessage(retryDelay, RetryLater)

        case Success(other) =>
          println(s"[Philosopher-$id] Unexpected reply for second fork: $other")
          scheduleMessage(retryDelay, RetryLater)
          
        case Failure(exception) =>
          println(s"[Philosopher-$id] Error securing second fork: ${exception.getMessage}")
          scheduleMessage(retryDelay, RetryLater)
      }
      
    } else {
      self ! StartEating
    }
  }
  
  private def releaseForks(): Unit = {
    if (hasFirstFork) {
      firstFork ! Release(id)
      hasFirstFork = false
    }
    if (hasSecondFork) {
      secondFork ! Release(id)
      hasSecondFork = false
    }
  }
  
  private def scheduleMessage(delay: FiniteDuration, message: Any): Unit = {
    currentTask.foreach(_.cancel())
    currentTask = Some(
      context.system.scheduler.scheduleOnce(delay) {
        self ! message
      }
    )
  }
}