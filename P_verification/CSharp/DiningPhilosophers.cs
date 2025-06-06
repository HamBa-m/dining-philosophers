using PChecker;
using PChecker.Runtime;
using PChecker.Runtime.StateMachines;
using PChecker.Runtime.Events;
using PChecker.Runtime.Exceptions;
using PChecker.Runtime.Logging;
using PChecker.Runtime.Values;
using PChecker.Runtime.Specifications;
using Monitor = PChecker.Runtime.Specifications.Monitor;
using System;
using PChecker.SystematicTesting;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 162, 219, 414, 1998
namespace PImplementation
{
}
namespace PImplementation
{
    internal partial class TryAcquire : Event
    {
        public TryAcquire() : base() {}
        public TryAcquire (PNamedTuple payload): base(payload){ }
        public override IPValue Clone() { return new TryAcquire();}
    }
}
namespace PImplementation
{
    internal partial class Release : Event
    {
        public Release() : base() {}
        public Release (PInt payload): base(payload){ }
        public override IPValue Clone() { return new Release();}
    }
}
namespace PImplementation
{
    internal partial class AcquireSuccess : Event
    {
        public AcquireSuccess() : base() {}
        public AcquireSuccess (IPValue payload): base(payload){ }
        public override IPValue Clone() { return new AcquireSuccess();}
    }
}
namespace PImplementation
{
    internal partial class AcquireFailed : Event
    {
        public AcquireFailed() : base() {}
        public AcquireFailed (IPValue payload): base(payload){ }
        public override IPValue Clone() { return new AcquireFailed();}
    }
}
namespace PImplementation
{
    internal partial class ePhilosopherAcquiredOneFork : Event
    {
        public ePhilosopherAcquiredOneFork() : base() {}
        public ePhilosopherAcquiredOneFork (PInt payload): base(payload){ }
        public override IPValue Clone() { return new ePhilosopherAcquiredOneFork();}
    }
}
namespace PImplementation
{
    internal partial class ePhilosopherReleasedForks : Event
    {
        public ePhilosopherReleasedForks() : base() {}
        public ePhilosopherReleasedForks (PInt payload): base(payload){ }
        public override IPValue Clone() { return new ePhilosopherReleasedForks();}
    }
}
namespace PImplementation
{
    internal partial class eSize : Event
    {
        public eSize() : base() {}
        public eSize (PInt payload): base(payload){ }
        public override IPValue Clone() { return new eSize();}
    }
}
namespace PImplementation
{
    internal partial class Fork : StateMachine
    {
        private PInt holder = ((PInt)0);
        private PBool isHeld = ((PBool)false);
        private PInt forkId = ((PInt)0);
        private PInt priority = ((PInt)0);
        public class ConstructorEvent : Event{public ConstructorEvent(IPValue val) : base(val) { }}
        
        protected override Event GetConstructorEvent(IPValue value) { return new ConstructorEvent((IPValue)value); }
        public Fork() {
            this.sends.Add(nameof(AcquireFailed));
            this.sends.Add(nameof(AcquireSuccess));
            this.sends.Add(nameof(Release));
            this.sends.Add(nameof(TryAcquire));
            this.sends.Add(nameof(ePhilosopherAcquiredOneFork));
            this.sends.Add(nameof(ePhilosopherReleasedForks));
            this.sends.Add(nameof(eSize));
            this.sends.Add(nameof(PHalt));
            this.receives.Add(nameof(AcquireFailed));
            this.receives.Add(nameof(AcquireSuccess));
            this.receives.Add(nameof(Release));
            this.receives.Add(nameof(TryAcquire));
            this.receives.Add(nameof(ePhilosopherAcquiredOneFork));
            this.receives.Add(nameof(ePhilosopherReleasedForks));
            this.receives.Add(nameof(eSize));
            this.receives.Add(nameof(PHalt));
        }
        
        public void Anon(Event currentMachine_dequeuedEvent)
        {
            Fork currentMachine = this;
            PInt p = (PInt)(gotoPayload ?? ((Event)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PInt TMP_tmp0 = ((PInt)0);
            TMP_tmp0 = (PInt)(-(((PInt)(1))));
            holder = TMP_tmp0;
            isHeld = (PBool)(((PBool)false));
            forkId = (PInt)(((PInt)(0)));
            priority = (PInt)(((PInt)((IPValue)p)?.Clone()));
        }
        public void Anon_1(Event currentMachine_dequeuedEvent)
        {
            Fork currentMachine = this;
            PNamedTuple payload = (PNamedTuple)(gotoPayload ?? ((Event)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PBool TMP_tmp0_1 = ((PBool)false);
            PInt TMP_tmp1 = ((PInt)0);
            PInt TMP_tmp2 = ((PInt)0);
            PMachineValue TMP_tmp3 = null;
            PMachineValue TMP_tmp4 = null;
            Event TMP_tmp5 = null;
            PMachineValue TMP_tmp6 = null;
            PMachineValue TMP_tmp7 = null;
            Event TMP_tmp8 = null;
            TMP_tmp0_1 = (PBool)(!(isHeld));
            if (TMP_tmp0_1)
            {
                TMP_tmp1 = (PInt)(((PNamedTuple)payload)["philosopherId"]);
                TMP_tmp2 = (PInt)(((PInt)((IPValue)TMP_tmp1)?.Clone()));
                holder = TMP_tmp2;
                isHeld = (PBool)(((PBool)true));
                TMP_tmp3 = (PMachineValue)(((PNamedTuple)payload)["requester"]);
                TMP_tmp4 = (PMachineValue)(((PMachineValue)((IPValue)TMP_tmp3)?.Clone()));
                TMP_tmp5 = (Event)(new AcquireSuccess(null));
                currentMachine.SendEvent(TMP_tmp4, (Event)TMP_tmp5);
                currentMachine.RaiseGotoStateEvent<NotAvailable>();
                return;
            }
            else
            {
                TMP_tmp6 = (PMachineValue)(((PNamedTuple)payload)["requester"]);
                TMP_tmp7 = (PMachineValue)(((PMachineValue)((IPValue)TMP_tmp6)?.Clone()));
                TMP_tmp8 = (Event)(new AcquireFailed(null));
                currentMachine.SendEvent(TMP_tmp7, (Event)TMP_tmp8);
            }
        }
        public void Anon_2(Event currentMachine_dequeuedEvent)
        {
            Fork currentMachine = this;
            PNamedTuple payload_1 = (PNamedTuple)(gotoPayload ?? ((Event)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PMachineValue TMP_tmp0_2 = null;
            PMachineValue TMP_tmp1_1 = null;
            Event TMP_tmp2_1 = null;
            TMP_tmp0_2 = (PMachineValue)(((PNamedTuple)payload_1)["requester"]);
            TMP_tmp1_1 = (PMachineValue)(((PMachineValue)((IPValue)TMP_tmp0_2)?.Clone()));
            TMP_tmp2_1 = (Event)(new AcquireFailed(null));
            currentMachine.SendEvent(TMP_tmp1_1, (Event)TMP_tmp2_1);
        }
        public void Anon_3(Event currentMachine_dequeuedEvent)
        {
            Fork currentMachine = this;
            PInt philosopherId = (PInt)(gotoPayload ?? ((Event)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PBool TMP_tmp0_3 = ((PBool)false);
            PBool TMP_tmp1_2 = ((PBool)false);
            PInt TMP_tmp2_2 = ((PInt)0);
            PInt TMP_tmp3_1 = ((PInt)0);
            TMP_tmp1_2 = (PBool)(((PBool)((IPValue)isHeld)?.Clone()));
            if (TMP_tmp1_2)
            {
                TMP_tmp0_3 = (PBool)((PValues.SafeEquals(holder,philosopherId)));
                TMP_tmp1_2 = (PBool)(((PBool)((IPValue)TMP_tmp0_3)?.Clone()));
            }
            if (TMP_tmp1_2)
            {
                TMP_tmp2_2 = (PInt)(-(((PInt)(1))));
                holder = TMP_tmp2_2;
                isHeld = (PBool)(((PBool)false));
                TMP_tmp3_1 = (PInt)(((PInt)((IPValue)priority)?.Clone()));
                currentMachine.RaiseGotoStateEvent<Available>(TMP_tmp3_1);
                return;
            }
        }
        [Start]
        [OnEntry(nameof(Anon))]
        [OnEventDoAction(typeof(TryAcquire), nameof(Anon_1))]
        class Available : State
        {
        }
        [OnEventDoAction(typeof(TryAcquire), nameof(Anon_2))]
        [OnEventDoAction(typeof(Release), nameof(Anon_3))]
        class NotAvailable : State
        {
        }
    }
}
namespace PImplementation
{
    internal partial class Philosopher : StateMachine
    {
        private PInt id = ((PInt)0);
        private PMachineValue firstFork = null;
        private PMachineValue secondFork = null;
        private PBool hasFirstFork = ((PBool)false);
        private PBool hasSecondFork = ((PBool)false);
        public class ConstructorEvent : Event{public ConstructorEvent(IPValue val) : base(val) { }}
        
        protected override Event GetConstructorEvent(IPValue value) { return new ConstructorEvent((IPValue)value); }
        public Philosopher() {
            this.sends.Add(nameof(AcquireFailed));
            this.sends.Add(nameof(AcquireSuccess));
            this.sends.Add(nameof(Release));
            this.sends.Add(nameof(TryAcquire));
            this.sends.Add(nameof(ePhilosopherAcquiredOneFork));
            this.sends.Add(nameof(ePhilosopherReleasedForks));
            this.sends.Add(nameof(eSize));
            this.sends.Add(nameof(PHalt));
            this.receives.Add(nameof(AcquireFailed));
            this.receives.Add(nameof(AcquireSuccess));
            this.receives.Add(nameof(Release));
            this.receives.Add(nameof(TryAcquire));
            this.receives.Add(nameof(ePhilosopherAcquiredOneFork));
            this.receives.Add(nameof(ePhilosopherReleasedForks));
            this.receives.Add(nameof(eSize));
            this.receives.Add(nameof(PHalt));
        }
        
        public void Anon_4(Event currentMachine_dequeuedEvent)
        {
            Philosopher currentMachine = this;
            PNamedTuple config = (PNamedTuple)(gotoPayload ?? ((Event)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PInt TMP_tmp0_4 = ((PInt)0);
            PInt TMP_tmp1_3 = ((PInt)0);
            PBool TMP_tmp2_3 = ((PBool)false);
            PInt TMP_tmp3_2 = ((PInt)0);
            PInt TMP_tmp4_1 = ((PInt)0);
            PBool TMP_tmp5_1 = ((PBool)false);
            PMachineValue TMP_tmp6_1 = null;
            PMachineValue TMP_tmp7_1 = null;
            PMachineValue TMP_tmp8_1 = null;
            PMachineValue TMP_tmp9 = null;
            PMachineValue TMP_tmp10 = null;
            PMachineValue TMP_tmp11 = null;
            PMachineValue TMP_tmp12 = null;
            PMachineValue TMP_tmp13 = null;
            PMachineValue TMP_tmp14 = null;
            PMachineValue TMP_tmp15 = null;
            PMachineValue TMP_tmp16 = null;
            PMachineValue TMP_tmp17 = null;
            TMP_tmp0_4 = (PInt)(((PNamedTuple)config)["id"]);
            TMP_tmp1_3 = (PInt)(((PInt)((IPValue)TMP_tmp0_4)?.Clone()));
            id = TMP_tmp1_3;
            TMP_tmp2_3 = (PBool)(((PNamedTuple)config)["enableResourceHierarchy"]);
            if (TMP_tmp2_3)
            {
                TMP_tmp3_2 = (PInt)(((PNamedTuple)config)["leftPriority"]);
                TMP_tmp4_1 = (PInt)(((PNamedTuple)config)["rightPriority"]);
                TMP_tmp5_1 = (PBool)((TMP_tmp3_2) < (TMP_tmp4_1));
                if (TMP_tmp5_1)
                {
                    TMP_tmp6_1 = (PMachineValue)(((PNamedTuple)config)["leftFork"]);
                    TMP_tmp7_1 = (PMachineValue)(((PMachineValue)((IPValue)TMP_tmp6_1)?.Clone()));
                    firstFork = TMP_tmp7_1;
                    TMP_tmp8_1 = (PMachineValue)(((PNamedTuple)config)["rightFork"]);
                    TMP_tmp9 = (PMachineValue)(((PMachineValue)((IPValue)TMP_tmp8_1)?.Clone()));
                    secondFork = TMP_tmp9;
                }
                else
                {
                    TMP_tmp10 = (PMachineValue)(((PNamedTuple)config)["rightFork"]);
                    TMP_tmp11 = (PMachineValue)(((PMachineValue)((IPValue)TMP_tmp10)?.Clone()));
                    firstFork = TMP_tmp11;
                    TMP_tmp12 = (PMachineValue)(((PNamedTuple)config)["leftFork"]);
                    TMP_tmp13 = (PMachineValue)(((PMachineValue)((IPValue)TMP_tmp12)?.Clone()));
                    secondFork = TMP_tmp13;
                }
            }
            else
            {
                TMP_tmp14 = (PMachineValue)(((PNamedTuple)config)["leftFork"]);
                TMP_tmp15 = (PMachineValue)(((PMachineValue)((IPValue)TMP_tmp14)?.Clone()));
                firstFork = TMP_tmp15;
                TMP_tmp16 = (PMachineValue)(((PNamedTuple)config)["rightFork"]);
                TMP_tmp17 = (PMachineValue)(((PMachineValue)((IPValue)TMP_tmp16)?.Clone()));
                secondFork = TMP_tmp17;
            }
            hasFirstFork = (PBool)(((PBool)false));
            hasSecondFork = (PBool)(((PBool)false));
            currentMachine.RaiseGotoStateEvent<Thinking>();
            return;
        }
        public void Anon_5(Event currentMachine_dequeuedEvent)
        {
            Philosopher currentMachine = this;
            currentMachine.RaiseGotoStateEvent<Hungry>();
            return;
        }
        public void Anon_6(Event currentMachine_dequeuedEvent)
        {
            Philosopher currentMachine = this;
            PMachineValue TMP_tmp0_5 = null;
            Event TMP_tmp1_4 = null;
            PMachineValue TMP_tmp2_4 = null;
            PInt TMP_tmp3_3 = ((PInt)0);
            PNamedTuple TMP_tmp4_2 = (new PNamedTuple(new string[]{"requester","philosopherId"},null, ((PInt)0)));
            TMP_tmp0_5 = (PMachineValue)(((PMachineValue)((IPValue)firstFork)?.Clone()));
            TMP_tmp1_4 = (Event)(new TryAcquire((new PNamedTuple(new string[]{"requester","philosopherId"},null, ((PInt)0)))));
            TMP_tmp2_4 = (PMachineValue)(currentMachine.self);
            TMP_tmp3_3 = (PInt)(((PInt)((IPValue)id)?.Clone()));
            TMP_tmp4_2 = (PNamedTuple)((new PNamedTuple(new string[]{"requester","philosopherId"}, TMP_tmp2_4, TMP_tmp3_3)));
            TMP_tmp1_4.Payload = TMP_tmp4_2;
            currentMachine.SendEvent(TMP_tmp0_5, (Event)TMP_tmp1_4);
        }
        public void Anon_7(Event currentMachine_dequeuedEvent)
        {
            Philosopher currentMachine = this;
            hasFirstFork = (PBool)(((PBool)true));
            currentMachine.Announce((Event)new ePhilosopherAcquiredOneFork(((PInt)0)), id);
            currentMachine.RaiseGotoStateEvent<TryToGetSecondFork>();
            return;
        }
        public void Anon_8(Event currentMachine_dequeuedEvent)
        {
            Philosopher currentMachine = this;
            currentMachine.RaiseGotoStateEvent<Hungry>();
            return;
        }
        public void Anon_9(Event currentMachine_dequeuedEvent)
        {
            Philosopher currentMachine = this;
            PMachineValue TMP_tmp0_6 = null;
            Event TMP_tmp1_5 = null;
            PMachineValue TMP_tmp2_5 = null;
            PInt TMP_tmp3_4 = ((PInt)0);
            PNamedTuple TMP_tmp4_3 = (new PNamedTuple(new string[]{"requester","philosopherId"},null, ((PInt)0)));
            TMP_tmp0_6 = (PMachineValue)(((PMachineValue)((IPValue)secondFork)?.Clone()));
            TMP_tmp1_5 = (Event)(new TryAcquire((new PNamedTuple(new string[]{"requester","philosopherId"},null, ((PInt)0)))));
            TMP_tmp2_5 = (PMachineValue)(currentMachine.self);
            TMP_tmp3_4 = (PInt)(((PInt)((IPValue)id)?.Clone()));
            TMP_tmp4_3 = (PNamedTuple)((new PNamedTuple(new string[]{"requester","philosopherId"}, TMP_tmp2_5, TMP_tmp3_4)));
            TMP_tmp1_5.Payload = TMP_tmp4_3;
            currentMachine.SendEvent(TMP_tmp0_6, (Event)TMP_tmp1_5);
        }
        public void Anon_10(Event currentMachine_dequeuedEvent)
        {
            Philosopher currentMachine = this;
            hasSecondFork = (PBool)(((PBool)true));
            currentMachine.RaiseGotoStateEvent<Eating>();
            return;
        }
        public void Anon_11(Event currentMachine_dequeuedEvent)
        {
            Philosopher currentMachine = this;
            currentMachine.RaiseGotoStateEvent<TryToGetSecondFork>();
            return;
        }
        public void Anon_12(Event currentMachine_dequeuedEvent)
        {
            Philosopher currentMachine = this;
            PMachineValue TMP_tmp0_7 = null;
            Event TMP_tmp1_6 = null;
            PInt TMP_tmp2_6 = ((PInt)0);
            PMachineValue TMP_tmp3_5 = null;
            Event TMP_tmp4_4 = null;
            PInt TMP_tmp5_2 = ((PInt)0);
            if (hasFirstFork)
            {
                TMP_tmp0_7 = (PMachineValue)(((PMachineValue)((IPValue)firstFork)?.Clone()));
                TMP_tmp1_6 = (Event)(new Release(((PInt)0)));
                TMP_tmp2_6 = (PInt)(((PInt)((IPValue)id)?.Clone()));
                TMP_tmp1_6.Payload = TMP_tmp2_6;
                currentMachine.SendEvent(TMP_tmp0_7, (Event)TMP_tmp1_6);
                hasFirstFork = (PBool)(((PBool)false));
            }
            if (hasSecondFork)
            {
                TMP_tmp3_5 = (PMachineValue)(((PMachineValue)((IPValue)secondFork)?.Clone()));
                TMP_tmp4_4 = (Event)(new Release(((PInt)0)));
                TMP_tmp5_2 = (PInt)(((PInt)((IPValue)id)?.Clone()));
                TMP_tmp4_4.Payload = TMP_tmp5_2;
                currentMachine.SendEvent(TMP_tmp3_5, (Event)TMP_tmp4_4);
                hasSecondFork = (PBool)(((PBool)false));
            }
            currentMachine.Announce((Event)new ePhilosopherReleasedForks(((PInt)0)), id);
            currentMachine.RaiseGotoStateEvent<Thinking>();
            return;
        }
        [Start]
        [OnEntry(nameof(Anon_4))]
        class Init : State
        {
        }
        [OnEntry(nameof(Anon_5))]
        class Thinking : State
        {
        }
        [OnEntry(nameof(Anon_6))]
        [OnEventDoAction(typeof(AcquireSuccess), nameof(Anon_7))]
        [OnEventDoAction(typeof(AcquireFailed), nameof(Anon_8))]
        class Hungry : State
        {
        }
        [OnEntry(nameof(Anon_9))]
        [OnEventDoAction(typeof(AcquireSuccess), nameof(Anon_10))]
        [OnEventDoAction(typeof(AcquireFailed), nameof(Anon_11))]
        class TryToGetSecondFork : State
        {
        }
        [OnEntry(nameof(Anon_12))]
        class Eating : State
        {
        }
    }
}
namespace PImplementation
{
    internal partial class Main : StateMachine
    {
        private PInt N = ((PInt)0);
        private PSeq Forks = new PSeq();
        private PSeq Philosophers = new PSeq();
        private PInt i = ((PInt)0);
        public class ConstructorEvent : Event{public ConstructorEvent(IPValue val) : base(val) { }}
        
        protected override Event GetConstructorEvent(IPValue value) { return new ConstructorEvent((IPValue)value); }
        public Main() {
            this.sends.Add(nameof(AcquireFailed));
            this.sends.Add(nameof(AcquireSuccess));
            this.sends.Add(nameof(Release));
            this.sends.Add(nameof(TryAcquire));
            this.sends.Add(nameof(ePhilosopherAcquiredOneFork));
            this.sends.Add(nameof(ePhilosopherReleasedForks));
            this.sends.Add(nameof(eSize));
            this.sends.Add(nameof(PHalt));
            this.receives.Add(nameof(AcquireFailed));
            this.receives.Add(nameof(AcquireSuccess));
            this.receives.Add(nameof(Release));
            this.receives.Add(nameof(TryAcquire));
            this.receives.Add(nameof(ePhilosopherAcquiredOneFork));
            this.receives.Add(nameof(ePhilosopherReleasedForks));
            this.receives.Add(nameof(eSize));
            this.receives.Add(nameof(PHalt));
            this.creates.Add(nameof(I_Fork));
            this.creates.Add(nameof(I_Philosopher));
        }
        
        public void Anon_13(Event currentMachine_dequeuedEvent)
        {
            Main currentMachine = this;
            PSeq TMP_tmp0_8 = new PSeq();
            PSeq TMP_tmp1_7 = new PSeq();
            PBool TMP_tmp2_7 = ((PBool)false);
            PBool TMP_tmp3_6 = ((PBool)false);
            PInt TMP_tmp4_5 = ((PInt)0);
            PMachineValue TMP_tmp5_3 = null;
            PInt TMP_tmp6_2 = ((PInt)0);
            PBool TMP_tmp7_2 = ((PBool)false);
            PBool TMP_tmp8_2 = ((PBool)false);
            PInt TMP_tmp9_1 = ((PInt)0);
            PMachineValue TMP_tmp10_1 = null;
            PInt TMP_tmp11_1 = ((PInt)0);
            PInt TMP_tmp12_1 = ((PInt)0);
            PInt TMP_tmp13_1 = ((PInt)0);
            PMachineValue TMP_tmp14_1 = null;
            PInt TMP_tmp15_1 = ((PInt)0);
            PInt TMP_tmp16_1 = ((PInt)0);
            PBool TMP_tmp17_1 = ((PBool)false);
            PNamedTuple TMP_tmp18 = (new PNamedTuple(new string[]{"id","leftFork","leftPriority","rightFork","rightPriority","enableResourceHierarchy"},((PInt)0), null, ((PInt)0), null, ((PInt)0), ((PBool)false)));
            PMachineValue TMP_tmp19 = null;
            PInt TMP_tmp20 = ((PInt)0);
            N = (PInt)(((PInt)(5)));
            TMP_tmp0_8 = (PSeq)(new PSeq());
            Forks = TMP_tmp0_8;
            TMP_tmp1_7 = (PSeq)(new PSeq());
            Philosophers = TMP_tmp1_7;
            currentMachine.Announce((Event)new eSize(((PInt)0)), N);
            i = (PInt)(((PInt)(0)));
            while (((PBool)true))
            {
                TMP_tmp2_7 = (PBool)((i) < (N));
                TMP_tmp3_6 = (PBool)(((PBool)((IPValue)TMP_tmp2_7)?.Clone()));
                if (TMP_tmp3_6)
                {
                }
                else
                {
                    break;
                }
                TMP_tmp4_5 = (PInt)(((PInt)((IPValue)i)?.Clone()));
                TMP_tmp5_3 = (PMachineValue)(currentMachine.CreateInterface<I_Fork>( currentMachine, TMP_tmp4_5));
                ((PSeq)Forks).Insert(((PInt)(0)), TMP_tmp5_3);
                TMP_tmp6_2 = (PInt)((i) + (((PInt)(1))));
                i = TMP_tmp6_2;
            }
            i = (PInt)(((PInt)(0)));
            while (((PBool)true))
            {
                TMP_tmp7_2 = (PBool)((i) < (N));
                TMP_tmp8_2 = (PBool)(((PBool)((IPValue)TMP_tmp7_2)?.Clone()));
                if (TMP_tmp8_2)
                {
                }
                else
                {
                    break;
                }
                TMP_tmp9_1 = (PInt)(((PInt)((IPValue)i)?.Clone()));
                TMP_tmp10_1 = (PMachineValue)(((PSeq)Forks)[i]);
                TMP_tmp11_1 = (PInt)(((PInt)((IPValue)i)?.Clone()));
                TMP_tmp12_1 = (PInt)((i) + (((PInt)(1))));
                TMP_tmp13_1 = (PInt)((TMP_tmp12_1) % (N));
                TMP_tmp14_1 = (PMachineValue)(((PSeq)Forks)[TMP_tmp13_1]);
                TMP_tmp15_1 = (PInt)((i) + (((PInt)(1))));
                TMP_tmp16_1 = (PInt)((TMP_tmp15_1) % (N));
                TMP_tmp17_1 = (PBool)(((PBool)false));
                TMP_tmp18 = (PNamedTuple)((new PNamedTuple(new string[]{"id","leftFork","leftPriority","rightFork","rightPriority","enableResourceHierarchy"}, TMP_tmp9_1, TMP_tmp10_1, TMP_tmp11_1, TMP_tmp14_1, TMP_tmp16_1, TMP_tmp17_1)));
                TMP_tmp19 = (PMachineValue)(currentMachine.CreateInterface<I_Philosopher>( currentMachine, TMP_tmp18));
                ((PSeq)Philosophers).Insert(((PInt)(0)), TMP_tmp19);
                TMP_tmp20 = (PInt)((i) + (((PInt)(1))));
                i = TMP_tmp20;
            }
        }
        [Start]
        [OnEntry(nameof(Anon_13))]
        class Init : State
        {
        }
    }
}
namespace PImplementation
{
    internal partial class Main_NODL : StateMachine
    {
        private PInt N_1 = ((PInt)0);
        private PSeq Forks_1 = new PSeq();
        private PSeq Philosophers_1 = new PSeq();
        private PInt i_1 = ((PInt)0);
        public class ConstructorEvent : Event{public ConstructorEvent(IPValue val) : base(val) { }}
        
        protected override Event GetConstructorEvent(IPValue value) { return new ConstructorEvent((IPValue)value); }
        public Main_NODL() {
            this.sends.Add(nameof(AcquireFailed));
            this.sends.Add(nameof(AcquireSuccess));
            this.sends.Add(nameof(Release));
            this.sends.Add(nameof(TryAcquire));
            this.sends.Add(nameof(ePhilosopherAcquiredOneFork));
            this.sends.Add(nameof(ePhilosopherReleasedForks));
            this.sends.Add(nameof(eSize));
            this.sends.Add(nameof(PHalt));
            this.receives.Add(nameof(AcquireFailed));
            this.receives.Add(nameof(AcquireSuccess));
            this.receives.Add(nameof(Release));
            this.receives.Add(nameof(TryAcquire));
            this.receives.Add(nameof(ePhilosopherAcquiredOneFork));
            this.receives.Add(nameof(ePhilosopherReleasedForks));
            this.receives.Add(nameof(eSize));
            this.receives.Add(nameof(PHalt));
            this.creates.Add(nameof(I_Fork));
            this.creates.Add(nameof(I_Philosopher));
        }
        
        public void Anon_14(Event currentMachine_dequeuedEvent)
        {
            Main_NODL currentMachine = this;
            PSeq TMP_tmp0_9 = new PSeq();
            PSeq TMP_tmp1_8 = new PSeq();
            PBool TMP_tmp2_8 = ((PBool)false);
            PBool TMP_tmp3_7 = ((PBool)false);
            PInt TMP_tmp4_6 = ((PInt)0);
            PMachineValue TMP_tmp5_4 = null;
            PInt TMP_tmp6_3 = ((PInt)0);
            PBool TMP_tmp7_3 = ((PBool)false);
            PBool TMP_tmp8_3 = ((PBool)false);
            PInt TMP_tmp9_2 = ((PInt)0);
            PMachineValue TMP_tmp10_2 = null;
            PInt TMP_tmp11_2 = ((PInt)0);
            PInt TMP_tmp12_2 = ((PInt)0);
            PInt TMP_tmp13_2 = ((PInt)0);
            PMachineValue TMP_tmp14_2 = null;
            PInt TMP_tmp15_2 = ((PInt)0);
            PInt TMP_tmp16_2 = ((PInt)0);
            PBool TMP_tmp17_2 = ((PBool)false);
            PNamedTuple TMP_tmp18_1 = (new PNamedTuple(new string[]{"id","leftFork","leftPriority","rightFork","rightPriority","enableResourceHierarchy"},((PInt)0), null, ((PInt)0), null, ((PInt)0), ((PBool)false)));
            PMachineValue TMP_tmp19_1 = null;
            PInt TMP_tmp20_1 = ((PInt)0);
            N_1 = (PInt)(((PInt)(5)));
            TMP_tmp0_9 = (PSeq)(new PSeq());
            Forks_1 = TMP_tmp0_9;
            TMP_tmp1_8 = (PSeq)(new PSeq());
            Philosophers_1 = TMP_tmp1_8;
            currentMachine.Announce((Event)new eSize(((PInt)0)), N_1);
            i_1 = (PInt)(((PInt)(0)));
            while (((PBool)true))
            {
                TMP_tmp2_8 = (PBool)((i_1) < (N_1));
                TMP_tmp3_7 = (PBool)(((PBool)((IPValue)TMP_tmp2_8)?.Clone()));
                if (TMP_tmp3_7)
                {
                }
                else
                {
                    break;
                }
                TMP_tmp4_6 = (PInt)(((PInt)((IPValue)i_1)?.Clone()));
                TMP_tmp5_4 = (PMachineValue)(currentMachine.CreateInterface<I_Fork>( currentMachine, TMP_tmp4_6));
                ((PSeq)Forks_1).Insert(((PInt)(0)), TMP_tmp5_4);
                TMP_tmp6_3 = (PInt)((i_1) + (((PInt)(1))));
                i_1 = TMP_tmp6_3;
            }
            i_1 = (PInt)(((PInt)(0)));
            while (((PBool)true))
            {
                TMP_tmp7_3 = (PBool)((i_1) < (N_1));
                TMP_tmp8_3 = (PBool)(((PBool)((IPValue)TMP_tmp7_3)?.Clone()));
                if (TMP_tmp8_3)
                {
                }
                else
                {
                    break;
                }
                TMP_tmp9_2 = (PInt)(((PInt)((IPValue)i_1)?.Clone()));
                TMP_tmp10_2 = (PMachineValue)(((PSeq)Forks_1)[i_1]);
                TMP_tmp11_2 = (PInt)(((PInt)((IPValue)i_1)?.Clone()));
                TMP_tmp12_2 = (PInt)((i_1) + (((PInt)(1))));
                TMP_tmp13_2 = (PInt)((TMP_tmp12_2) % (N_1));
                TMP_tmp14_2 = (PMachineValue)(((PSeq)Forks_1)[TMP_tmp13_2]);
                TMP_tmp15_2 = (PInt)((i_1) + (((PInt)(1))));
                TMP_tmp16_2 = (PInt)((TMP_tmp15_2) % (N_1));
                TMP_tmp17_2 = (PBool)(((PBool)true));
                TMP_tmp18_1 = (PNamedTuple)((new PNamedTuple(new string[]{"id","leftFork","leftPriority","rightFork","rightPriority","enableResourceHierarchy"}, TMP_tmp9_2, TMP_tmp10_2, TMP_tmp11_2, TMP_tmp14_2, TMP_tmp16_2, TMP_tmp17_2)));
                TMP_tmp19_1 = (PMachineValue)(currentMachine.CreateInterface<I_Philosopher>( currentMachine, TMP_tmp18_1));
                ((PSeq)Philosophers_1).Insert(((PInt)(0)), TMP_tmp19_1);
                TMP_tmp20_1 = (PInt)((i_1) + (((PInt)(1))));
                i_1 = TMP_tmp20_1;
            }
        }
        [Start]
        [OnEntry(nameof(Anon_14))]
        class Init : State
        {
        }
    }
}
namespace PImplementation
{
    internal partial class DeadlockDetector : Monitor
    {
        private PSet philosophersWithOneFork = new PSet();
        private PInt N_2 = ((PInt)0);
        static DeadlockDetector() {
            observes.Add(nameof(ePhilosopherAcquiredOneFork));
            observes.Add(nameof(ePhilosopherReleasedForks));
            observes.Add(nameof(eSize));
        }
        
        public void Anon_15(Event currentMachine_dequeuedEvent)
        {
            DeadlockDetector currentMachine = this;
            PSet TMP_tmp0_10 = new PSet();
            TMP_tmp0_10 = (PSet)(new PSet());
            philosophersWithOneFork = TMP_tmp0_10;
            N_2 = (PInt)(((PInt)(0)));
        }
        public void Anon_16(Event currentMachine_dequeuedEvent)
        {
            DeadlockDetector currentMachine = this;
            PInt size = (PInt)(gotoPayload ?? ((Event)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            N_2 = (PInt)(((PInt)((IPValue)size)?.Clone()));
        }
        public void Anon_17(Event currentMachine_dequeuedEvent)
        {
            DeadlockDetector currentMachine = this;
            PInt philosopherId_1 = (PInt)(gotoPayload ?? ((Event)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PInt TMP_tmp0_11 = ((PInt)0);
            PInt TMP_tmp1_9 = ((PInt)0);
            PBool TMP_tmp2_9 = ((PBool)false);
            PString TMP_tmp3_8 = ((PString)"");
            PInt TMP_tmp4_7 = ((PInt)0);
            PString TMP_tmp5_5 = ((PString)"");
            PString TMP_tmp6_4 = ((PString)"");
            TMP_tmp0_11 = (PInt)(((PInt)((IPValue)philosopherId_1)?.Clone()));
            ((PSet)philosophersWithOneFork).Add(TMP_tmp0_11);
            TMP_tmp1_9 = (PInt)(((PInt)(philosophersWithOneFork).Count));
            TMP_tmp2_9 = (PBool)((TMP_tmp1_9) < (N_2));
            if (TMP_tmp2_9)
            {
            }
            else
            {
                TMP_tmp3_8 = (PString)(((PString) String.Format("DeadlockSpec.p:18:17")));
                TMP_tmp4_7 = (PInt)(((PInt)((IPValue)N_2)?.Clone()));
                TMP_tmp5_5 = (PString)(((PString) String.Format("DEADLOCK DETECTED! All {0} philosophers hold one fork simultaneously",TMP_tmp4_7)));
                TMP_tmp6_4 = (PString)(((PString) String.Format("{0} {1}",TMP_tmp3_8,TMP_tmp5_5)));
                currentMachine.Assert(TMP_tmp2_9,"Assertion Failed: " + TMP_tmp6_4);
            }
        }
        public void Anon_18(Event currentMachine_dequeuedEvent)
        {
            DeadlockDetector currentMachine = this;
            PInt philosopherId_2 = (PInt)(gotoPayload ?? ((Event)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            ((PSet)philosophersWithOneFork).Remove(philosopherId_2);
        }
        [Start]
        [OnEntry(nameof(Anon_15))]
        [OnEventDoAction(typeof(eSize), nameof(Anon_16))]
        [OnEventDoAction(typeof(ePhilosopherAcquiredOneFork), nameof(Anon_17))]
        [OnEventDoAction(typeof(ePhilosopherReleasedForks), nameof(Anon_18))]
        class Monitoring : State
        {
        }
    }
}
namespace PImplementation
{
    public class DeadLockImpl {
        public static void InitializeLinkMap() {
            PModule.linkMap.Clear();
            PModule.linkMap[nameof(I_Main)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_Main)].Add(nameof(I_Fork), nameof(I_Fork));
            PModule.linkMap[nameof(I_Main)].Add(nameof(I_Philosopher), nameof(I_Philosopher));
            PModule.linkMap[nameof(I_Philosopher)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_Fork)] = new Dictionary<string, string>();
        }
        
        public static void InitializeInterfaceDefMap() {
            PModule.interfaceDefinitionMap.Clear();
            PModule.interfaceDefinitionMap.Add(nameof(I_Main), typeof(Main));
            PModule.interfaceDefinitionMap.Add(nameof(I_Philosopher), typeof(Philosopher));
            PModule.interfaceDefinitionMap.Add(nameof(I_Fork), typeof(Fork));
        }
        
        public static void InitializeMonitorObserves() {
            PModule.monitorObserves.Clear();
            PModule.monitorObserves[nameof(DeadlockDetector)] = new List<string>();
            PModule.monitorObserves[nameof(DeadlockDetector)].Add(nameof(ePhilosopherAcquiredOneFork));
            PModule.monitorObserves[nameof(DeadlockDetector)].Add(nameof(ePhilosopherReleasedForks));
            PModule.monitorObserves[nameof(DeadlockDetector)].Add(nameof(eSize));
        }
        
        public static void InitializeMonitorMap(ControlledRuntime runtime) {
            PModule.monitorMap.Clear();
            PModule.monitorMap[nameof(I_Main)] = new List<Type>();
            PModule.monitorMap[nameof(I_Main)].Add(typeof(DeadlockDetector));
            PModule.monitorMap[nameof(I_Philosopher)] = new List<Type>();
            PModule.monitorMap[nameof(I_Philosopher)].Add(typeof(DeadlockDetector));
            PModule.monitorMap[nameof(I_Fork)] = new List<Type>();
            PModule.monitorMap[nameof(I_Fork)].Add(typeof(DeadlockDetector));
            runtime.RegisterMonitor<DeadlockDetector>();
        }
        
        
        [PChecker.SystematicTesting.Test]
        public static void Execute(ControlledRuntime runtime) {
            runtime.RegisterLog(new PCheckerLogTextFormatter());
            runtime.RegisterLog(new PCheckerLogJsonFormatter());
            PModule.runtime = runtime;
            PHelper.InitializeInterfaces();
            PHelper.InitializeEnums();
            InitializeLinkMap();
            InitializeInterfaceDefMap();
            InitializeMonitorMap(runtime);
            InitializeMonitorObserves();
            runtime.CreateStateMachine(typeof(Main), "Main");
        }
    }
}
namespace PImplementation
{
    public class NoDeadLockImpl {
        public static void InitializeLinkMap() {
            PModule.linkMap.Clear();
            PModule.linkMap[nameof(I_Main_NODL)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_Main_NODL)].Add(nameof(I_Fork), nameof(I_Fork));
            PModule.linkMap[nameof(I_Main_NODL)].Add(nameof(I_Philosopher), nameof(I_Philosopher));
            PModule.linkMap[nameof(I_Philosopher)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_Fork)] = new Dictionary<string, string>();
        }
        
        public static void InitializeInterfaceDefMap() {
            PModule.interfaceDefinitionMap.Clear();
            PModule.interfaceDefinitionMap.Add(nameof(I_Main_NODL), typeof(Main_NODL));
            PModule.interfaceDefinitionMap.Add(nameof(I_Philosopher), typeof(Philosopher));
            PModule.interfaceDefinitionMap.Add(nameof(I_Fork), typeof(Fork));
        }
        
        public static void InitializeMonitorObserves() {
            PModule.monitorObserves.Clear();
            PModule.monitorObserves[nameof(DeadlockDetector)] = new List<string>();
            PModule.monitorObserves[nameof(DeadlockDetector)].Add(nameof(ePhilosopherAcquiredOneFork));
            PModule.monitorObserves[nameof(DeadlockDetector)].Add(nameof(ePhilosopherReleasedForks));
            PModule.monitorObserves[nameof(DeadlockDetector)].Add(nameof(eSize));
        }
        
        public static void InitializeMonitorMap(ControlledRuntime runtime) {
            PModule.monitorMap.Clear();
            PModule.monitorMap[nameof(I_Main_NODL)] = new List<Type>();
            PModule.monitorMap[nameof(I_Main_NODL)].Add(typeof(DeadlockDetector));
            PModule.monitorMap[nameof(I_Philosopher)] = new List<Type>();
            PModule.monitorMap[nameof(I_Philosopher)].Add(typeof(DeadlockDetector));
            PModule.monitorMap[nameof(I_Fork)] = new List<Type>();
            PModule.monitorMap[nameof(I_Fork)].Add(typeof(DeadlockDetector));
            runtime.RegisterMonitor<DeadlockDetector>();
        }
        
        
        [PChecker.SystematicTesting.Test]
        public static void Execute(ControlledRuntime runtime) {
            runtime.RegisterLog(new PCheckerLogTextFormatter());
            runtime.RegisterLog(new PCheckerLogJsonFormatter());
            PModule.runtime = runtime;
            PHelper.InitializeInterfaces();
            PHelper.InitializeEnums();
            InitializeLinkMap();
            InitializeInterfaceDefMap();
            InitializeMonitorMap(runtime);
            InitializeMonitorObserves();
            runtime.CreateStateMachine(typeof(Main_NODL), "Main_NODL");
        }
    }
}
// TODO: NamedModule Main_1
// TODO: NamedModule Main_NODL_1
// TODO: NamedModule Philosopher_1
// TODO: NamedModule Fork_1
namespace PImplementation
{
    public class I_Fork : PMachineValue {
        public I_Fork (StateMachineId machine, List<string> permissions) : base(machine, permissions) { }
    }
    
    public class I_Philosopher : PMachineValue {
        public I_Philosopher (StateMachineId machine, List<string> permissions) : base(machine, permissions) { }
    }
    
    public class I_Main : PMachineValue {
        public I_Main (StateMachineId machine, List<string> permissions) : base(machine, permissions) { }
    }
    
    public class I_Main_NODL : PMachineValue {
        public I_Main_NODL (StateMachineId machine, List<string> permissions) : base(machine, permissions) { }
    }
    
    public partial class PHelper {
        public static void InitializeInterfaces() {
            PInterfaces.Clear();
            PInterfaces.AddInterface(nameof(I_Fork), nameof(AcquireFailed), nameof(AcquireSuccess), nameof(Release), nameof(TryAcquire), nameof(ePhilosopherAcquiredOneFork), nameof(ePhilosopherReleasedForks), nameof(eSize), nameof(PHalt));
            PInterfaces.AddInterface(nameof(I_Philosopher), nameof(AcquireFailed), nameof(AcquireSuccess), nameof(Release), nameof(TryAcquire), nameof(ePhilosopherAcquiredOneFork), nameof(ePhilosopherReleasedForks), nameof(eSize), nameof(PHalt));
            PInterfaces.AddInterface(nameof(I_Main), nameof(AcquireFailed), nameof(AcquireSuccess), nameof(Release), nameof(TryAcquire), nameof(ePhilosopherAcquiredOneFork), nameof(ePhilosopherReleasedForks), nameof(eSize), nameof(PHalt));
            PInterfaces.AddInterface(nameof(I_Main_NODL), nameof(AcquireFailed), nameof(AcquireSuccess), nameof(Release), nameof(TryAcquire), nameof(ePhilosopherAcquiredOneFork), nameof(ePhilosopherReleasedForks), nameof(eSize), nameof(PHalt));
        }
    }
    
}
namespace PImplementation
{
    public partial class PHelper {
        public static void InitializeEnums() {
            PEnum.Clear();
        }
    }
    
}
#pragma warning restore 162, 219, 414
