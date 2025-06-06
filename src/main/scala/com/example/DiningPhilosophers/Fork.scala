package com.example.DiningPhilosophers

import akka.actor.Actor

object ForkMessages {
  case class TryAcquire(philosopherId: Int)
  case class Release(philosopherId: Int)
  case object AcquireSuccess
  case object AcquireFailed
}

/**
 * Fork Resource Unit - Managed for single-philosopher use at a time
 * Functions within the Fork Coordination Hub
 * Assigned a priority to support the Resource Hierarchy deadlock avoidance method
 */
class Fork(priority: Int) extends Actor {
  import ForkMessages._
  
  private var holder: Option[Int] = None
  private val forkId = self.path.name.stripPrefix("fork-").toInt
  
  override def preStart(): Unit = {
    println(s"[Fork-$forkId] Launched with priority rank $priority")
  }
  
  def receive: Receive = {
    case TryAcquire(philosopherId) =>
      holder match {
        case None =>
          holder = Some(philosopherId)
          sender() ! AcquireSuccess
          println(s"[Fork-$forkId] Taken by Philosopher $philosopherId")
          
        case Some(currentHolder) =>
          sender() ! AcquireFailed
          println(s"[Fork-$forkId] Blocked for Philosopher $philosopherId (held by $currentHolder)")
      }
    
    case Release(philosopherId) =>
      holder match {
        case Some(currentHolder) if currentHolder == philosopherId =>
          holder = None
          println(s"[Fork-$forkId] Freed by Philosopher $philosopherId")
          
        case Some(currentHolder) =>
          println(s"[Fork-$forkId] ERROR: Philosopher $philosopherId tried to free a fork held by $currentHolder")
          
        case None =>
          println(s"[Fork-$forkId] ERROR: Philosopher $philosopherId tried to free an unheld fork")
      }
  }
}