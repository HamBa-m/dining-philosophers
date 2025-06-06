package com.example.DiningPhilosophers

import akka.actor.{ActorRef, ActorSystem, Props}
import com.typesafe.config.ConfigFactory
import scala.concurrent.Await
import scala.concurrent.duration._

/**
 * Fork Coordination Hub - Sets up and supervises fork actors
 * Launch this before initiating the philosopher processes
 */
object ForksApp extends App {

  val forkCount: Int = if (args.length > 0) args(0).toInt else 5
  require(forkCount >= 2, s"Minimum of 2 forks required, received $forkCount")
  
  println(s"Activating Fork Coordination Hub with $forkCount forks")
  println("Hit ENTER to shut down the fork coordination hub...")
  
  // Configuration for fork system with remote access
  val forksPort: Int = 2552
  val forksConfig = ConfigFactory
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
            canonical.port = $forksPort
          }
        }
        
        # Minimize log verbosity
        loglevel = "INFO"
        stdout-loglevel = "INFO"
        log-dead-letters = 10
        log-dead-letters-during-shutdown = off
      }
      """)

  val forkSystem = ActorSystem("ForkCoordinationHub", forksConfig)

  try {
    // Initialize fork actors with assigned priorities
    val forks: Seq[ActorRef] =
      (0 until forkCount).map(i => forkSystem.actorOf(Props(new Fork(i)), s"fork-$i"))
    
    println(s"Deployed $forkCount forks with priority assignments:")
    forks.zipWithIndex.foreach { case (fork, i) =>
      println(s"  Fork $i (priority $i): ${fork.path}")
    }
    println()
    println("Fork Coordination Hub is ready for philosopher interactions...")

    Await.ready(forkSystem.whenTerminated, Duration.Inf)

  } finally {
    println("Shutting down Fork Coordination Hub...")
    forkSystem.terminate()
  }
}