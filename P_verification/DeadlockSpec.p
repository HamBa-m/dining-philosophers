spec DeadlockDetector observes ePhilosopherAcquiredOneFork, ePhilosopherAcquiredTwoForks, eSize {
    var philosophersWithOneFork: set[int];
    var N: int;
    
    start state Monitoring {
        entry {
            philosophersWithOneFork = default(set[int]);
            N = 0;
        }

        on eSize do (size: int) {
            N = size;
        }
        
        on ePhilosopherAcquiredOneFork do (philosopherId: int) {
            philosophersWithOneFork += (philosopherId);
            assert sizeof(philosophersWithOneFork) < N,
                format("DEADLOCK DETECTED! All {0} philosophers hold one fork simultaneously", N);
        }
        
        on ePhilosopherAcquiredTwoForks do (philosopherId: int) {
            philosophersWithOneFork -= (philosopherId);
        }
    }
}