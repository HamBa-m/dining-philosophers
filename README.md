# Dining Philosophers Problem

Distributed Akka implementation in Scala and formal verification in P language demonstrating deadlock scenarios and a solution for prevention using the Resource Hierarchy approach.

## Problem
Classic concurrency problem: 5 philosophers need 2 adjacent forks to eat. All grabbing left fork first → deadlock.

## Implementation
- **Scala**: Distributed Akka actors across 2 JVMs (ForksSystem + PhilosophersSystem)
- **P Language**: State machine modeling with exhaustive state space exploration

## Quick Start

### Scala (Run in separate terminals)
```bash
# Terminal 1: Start forks system first
sbt "runMain com.example.DiningPhilosophers.ForksApp n"

# Terminal 2: Start philosophers
sbt "runMain com.example.DiningPhilosophers.PhilosophersApp n false"  # With deadlock
sbt "runMain com.example.DiningPhilosophers.PhilosophersApp n true"   # Deadlock-free
```

### P Language Verification
```bash
cd P_verification
p.exe compile --pproj DiningPhilosophers.pproj --outdir .
p.exe check -tc DeadLockImpl -s 10      # Detects deadlock
p.exe check -tc NoDeadLockImpl -s 10    # Verifies deadlock-free
```

## Solution Strategy
**Resource Hierarchy approach**: Each fork is assigned a unique priority (its index). Philosophers always acquire the fork with the lower priority first, ensuring no circular wait condition and preventing deadlock.

## What You'll Observe

**With deadlock (false)**:
- Philosophers acquire left fork first, then right fork.
- System may hang when all philosophers hold one fork simultaneously.
- Console shows acquisition attempts but no eating in deadlock case.

**Without deadlock (true)**:
- Philosophers use the Resource Hierarchy approach, acquiring the lower-priority fork first.
- Continuous cycle of thinking→eating→thinking.
- P verification explores all possible execution paths systematically.

Replace `n` with desired number of philosophers (≥2).