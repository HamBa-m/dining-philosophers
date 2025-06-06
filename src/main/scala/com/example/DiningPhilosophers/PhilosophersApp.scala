package com.example.DiningPhilosophers

import akka.actor.{ActorRef, ActorSystem, Props}
import com.typesafe.config.ConfigFactory
import scala.concurrent.Await
import scala.concurrent.duration._

/**
 * Philosopher Operations Center - Deploys philosopher actors interfacing with remote fork units
 * Confirm the Fork Coordination Hub is active before starting this!
 */
object PhilosophersApp extends App {

  val philosopherCount: Int = if (args.length > 0) args(0).toInt else 5
  require(philosopherCount >= 2, s"At least 2 philosophers needed, got $philosopherCount")

  val useHierarchySolution: Boolean = if (args.length > 1) args(1).toBoolean else false
  println("Applying Hierarchy-based deadlock prevention: " + (if (useHierarchySolution) "Active" else "Inactive"))
  
  println(s"Launching Philosopher Operations Center with $philosopherCount philosophers")
  println("Ensure the Fork Coordination Hub is operational!")
  println("Press ENTER to cease philosopher operations...")
  
  // Configuration for philosopher system
  val philosophersPort: Int = 2553
  val philosophersConfig = ConfigFactory
    .parseString(
      s"""
      akka {
        actor {
          provider = "akka.remote.RemoteActorRefProvider"
          
          serializers {
            jackson-json = "akka.serialization.jackson.JacksonJsonSerializer"
          }

          serialization-bindings {
            "com.example.DiningPhilosophers.ForkMessages$$TryAcquire" = jackson-json
            "com.example.DiningPhilosophers.ForkMessages$$Release" = jackson-json
            "com.example.DiningPhilosophers.ForkMessages$$AcquireSuccess$$" = jackson-json
            "com.example.DiningPhilosophers.ForkMessages$$AcquireFailed$$" = jackson-json

            "com.example.DiningPhilosophers.PhilosopherMessages$$StartThinking$$" = jackson-json
            "com.example.DiningPhilosophers.PhilosopherMessages$$FinishedThinking$$" = jackson-json
            "com.example.DiningPhilosophers.PhilosopherMessages$$TryToEat$$" = jackson-json
            "com.example.DiningPhilosophers.PhilosopherMessages$$StartEating$$" = jackson-json
            "com.example.DiningPhilosophers.PhilosopherMessages$$FinishedEating$$" = jackson-json
            "com.example.DiningPhilosophers.PhilosopherMessages$$RetryLater$$" = jackson-json
          }
        }
        
        remote {
          artery {
            canonical.hostname = "localhost"
            canonical.port = $philosophersPort
          }
        }
        
        # Suppress excessive logging
        loglevel = "INFO"
        stdout-loglevel = "INFO"
        log-dead-letters = 10
        log-dead-letters-during-shutdown = off
      }
      """)

  val philosopherSystem = ActorSystem("PhilosopherOperationsCenter", philosophersConfig)

  try {
    val forksPort: Int = 2552

    // Deploy philosopher actors with remote fork links and priorities
    val philosophers: Seq[ActorRef] =
      (0 until philosopherCount).map { i =>
        val leftPriority = i
        val rightPriority = (i + 1) % philosopherCount
        val left = philosopherSystem.actorSelection(s"akka://ForkCoordinationHub@localhost:$forksPort/user/fork-$i")
        val right = philosopherSystem.actorSelection(s"akka://ForkCoordinationHub@localhost:$forksPort/user/fork-${(i + 1) % philosopherCount}")
        philosopherSystem.actorOf(Props(new Philosopher(i, left, leftPriority, right, rightPriority, useHierarchySolution)), s"phil-$i")
      }
    
    println(s"Configured $philosopherCount philosophers with fork priorities:")
    philosophers.foreach { philosopher =>
      val philId = philosopher.path.name.stripPrefix("phil-").toInt
      val leftIndex = philId
      val rightIndex = (philId + 1) % philosopherCount
      val leftForkPath = s"akka://ForkCoordinationHub@localhost:$forksPort/user/fork-$leftIndex"
      val rightForkPath = s"akka://ForkCoordinationHub@localhost:$forksPort/user/fork-$rightIndex"
      println(s"  Philosopher $philId: left=$leftForkPath (priority $leftIndex), right=$rightForkPath (priority $rightIndex)")
    }
    println()
    
    // Start all philosophers thinking
    Thread.sleep(2000) // Wait for remote connections to establish
    philosophers.foreach(_ ! PhilosopherMessages.StartThinking)
    println("Philosophers are now engaged in their initial contemplation phase...")
    
    Await.ready(philosopherSystem.whenTerminated, Duration.Inf)
    
  } finally {
    println("Closing down Philosopher Operations Center...")
    philosopherSystem.terminate()
  }
}