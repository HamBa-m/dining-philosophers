// Fork interaction events
event TryAcquire : (requester: machine, philosopherId: int);
event Release : int;
event AcquireSuccess;
event AcquireFailed;

// Monitoring events for specification
event ePhilosopherAcquiredOneFork : int;
event ePhilosopherReleasedForks : int;
event eSize : int;

type PhilosopherConfig = (id: int, leftFork: machine, leftPriority: int, rightFork: machine, rightPriority: int, enableResourceHierarchy: bool);

// Fork machine
machine Fork {
    var holder: int;
    var isHeld: bool;
    var forkId: int;
    var priority: int;
    
    start state Available {
        entry (p: int) {
            holder = -1;
            isHeld = false;
            forkId = 0;
            priority = p;
        }
        
        on TryAcquire do (payload: (requester: machine, philosopherId: int)) {
            if (!isHeld) {
                holder = payload.philosopherId;
                isHeld = true;
                send payload.requester, AcquireSuccess;
                goto NotAvailable;
            } else {
                send payload.requester, AcquireFailed;
            }
        }
    }
    
    state NotAvailable {
        on TryAcquire do (payload: (requester: machine, philosopherId: int)) {
            send payload.requester, AcquireFailed;
        }
        
        on Release do (philosopherId: int) {
            if (isHeld && holder == philosopherId) {
                holder = -1;
                isHeld = false;
                goto Available, priority;  // Fixed: Provide the priority parameter
            }
        }
    }
}

machine Philosopher {
    var id: int;
    var firstFork: machine;
    var secondFork: machine;
    var hasFirstFork: bool;
    var hasSecondFork: bool;
    
    start state Init {
        entry (config: PhilosopherConfig) {
            id = config.id;
            if (config.enableResourceHierarchy) {
                if (config.leftPriority < config.rightPriority) {
                    firstFork = config.leftFork;
                    secondFork = config.rightFork;
                } else {
                    firstFork = config.rightFork;
                    secondFork = config.leftFork;
                }
            } else {
                firstFork = config.leftFork;
                secondFork = config.rightFork;
            }
            hasFirstFork = false;
            hasSecondFork = false;
            goto Thinking;
        }
    }
    
    state Thinking {
        entry {
            goto Hungry;
        }   
    }
    
    state Hungry {
        entry {
            send firstFork, TryAcquire, (requester = this, philosopherId = id);
        }
        
        on AcquireSuccess do {
            hasFirstFork = true;
            announce ePhilosopherAcquiredOneFork, id;
            goto TryToGetSecondFork;
        }
        
        on AcquireFailed do {
            goto Hungry;
        }
    }

    state TryToGetSecondFork {
        entry {
            send secondFork, TryAcquire, (requester = this, philosopherId = id);
        }
        
        on AcquireSuccess do {
            hasSecondFork = true;
            goto Eating;
        }
        
        on AcquireFailed do { 
            goto TryToGetSecondFork;
        }
    }
    
    state Eating {
        entry {
            if (hasFirstFork) {
                send firstFork, Release, id;
                hasFirstFork = false;
            }
            if (hasSecondFork) {
                send secondFork, Release, id;
                hasSecondFork = false;
            }
            announce ePhilosopherReleasedForks, id;
            goto Thinking;
        }
    }
}

// Dining Scenario Orchestrator - Configures a deadlock-susceptible dining environment
machine Main {
    var N: int;
    var Forks: seq[machine];
    var Philosophers: seq[machine];
    var i: int;

    start state Init {
        entry {
            N = 5;
            Forks = default(seq[machine]);
            Philosophers = default(seq[machine]);
            
            announce eSize, N;
            
            // Set up fork units with priority levels
            i = 0;
            while (i < N) {
                Forks += (0, new Fork(i));
                i = i + 1;
            }

            // Deploy philosophers without hierarchy-based deadlock prevention
            i = 0;
            while (i < N) {
                Philosophers += (0, new Philosopher((
                    id = i, 
                    leftFork = Forks[i], 
                    leftPriority = i,
                    rightFork = Forks[(i + 1) % N],
                    rightPriority = (i + 1) % N,
                    enableResourceHierarchy = false
                )));
                i = i + 1;
            }
        }
    }
}

// Dining Scenario Orchestrator - Configures a deadlock-free dining environment with priority hierarchy
machine Main_NODL {
    var N: int;
    var Forks: seq[machine];
    var Philosophers: seq[machine];
    var i: int;

    start state Init {
        entry {
            N = 5;
            Forks = default(seq[machine]);
            Philosophers = default(seq[machine]);
            
            announce eSize, N;
            
            // Set up fork units with priority levels
            i = 0;
            while (i < N) {
                Forks += (0, new Fork(i));
                i = i + 1;
            }

            // Deploy philosophers with hierarchy-based deadlock prevention
            i = 0;
            while (i < N) {
                Philosophers += (0, new Philosopher((
                    id = i, 
                    leftFork = Forks[i], 
                    leftPriority = i,
                    rightFork = Forks[(i + 1) % N],
                    rightPriority = (i + 1) % N,
                    enableResourceHierarchy = true
                )));
                i = i + 1;
            }
        }
    }
}

module Main = { Main };
module Main_NODL = { Main_NODL };
module Philosopher = { Philosopher };
module Fork = { Fork };

test DeadLockImpl [main=Main]: assert DeadlockDetector in (union Main, Philosopher, Fork);
test NoDeadLockImpl [main=Main_NODL]: assert DeadlockDetector in (union Main_NODL, Philosopher, Fork);