// Decompiled with JetBrains decompiler
// Type: Retyped.p2
// Assembly: Retyped.p2, Version=0.7.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F58DD0-679F-4747-8C2E-08B610EA14D0
// Assembly location: C:\Users\thom\Downloads\retyped.p2.0.7.6733\lib\net40\Retyped.p2.dll
// Compiler-generated code is shown

using System;

namespace H5.Core
{
    [Scope]
    [Name("p2")]
    public static class p2
    {
        [Scope]
        [Name("p2")]
        [ExportedAs("p2", IsExportAssign = true)]
        public static class p22
        {
            public class AABB : IObject
            {
                public extern AABB();

                public extern AABB(p2.p22.AABB.Config options);

                public virtual extern void setFromPoints(
                  double[][] points,
                  double[] position,
                  double angle,
                  double skinSize);

                public virtual extern void copy(p2.p22.AABB aabb);

                public virtual extern void extend(p2.p22.AABB aabb);

                public virtual extern bool overlaps(p2.p22.AABB aabb);

                [ObjectLiteral]
                public class Config : IObject
                {
                    public double[] upperBound { get; set; }

                    public double[] lowerBound { get; set; }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class Broadphase : IObject
            {
                public static double AABB { get; set; }

                public static double BOUNDING_CIRCLE { get; set; }

                public static double NAIVE { get; set; }
                
                public static double SAP { get; set; }

                public static extern bool boundingRadiusCheck(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public static extern bool aabbCheck(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public static extern bool canCollide(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern Broadphase(double type);

                public virtual double type { get; set; }

                public virtual p2.p22.Body[] result { get; set; }

                public virtual p2.p22.World world { get; set; }

                public virtual double boundingVolumeType { get; set; }

                public virtual extern void setWorld(p2.p22.World world);

                public virtual extern p2.p22.Body[] getCollisionPairs(p2.p22.World world);

                public virtual extern bool boundingVolumeCheck(p2.p22.Body bodyA, p2.p22.Body bodyB);
            }

            public class GridBroadphase : p2.p22.Broadphase
            {
                public extern GridBroadphase();

                public extern GridBroadphase(p2.p22.GridBroadphase.Config options);

                public virtual double xmin { get; set; }

                public virtual double xmax { get; set; }

                public virtual double ymin { get; set; }

                public virtual double ymax { get; set; }

                public virtual double nx { get; set; }

                public virtual double ny { get; set; }

                public virtual double binsizeX { get; set; }

                public virtual double binsizeY { get; set; }

                public new static double AABB { get; set; }

                public new static double BOUNDING_CIRCLE { get; set; }

                public new static double NAIVE { get; set; }

                public new static double SAP { get; set; }

                public new static extern bool boundingRadiusCheck(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public new static extern bool aabbCheck(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public new static extern bool canCollide(p2.p22.Body bodyA, p2.p22.Body bodyB);

                [ObjectLiteral]
                public class Config : IObject
                {
                    public double? xmin { get; set; }

                    public double? xmax { get; set; }

                    public double? ymin { get; set; }

                    public double? ymax { get; set; }

                    public double? nx { get; set; }

                    public double? ny { get; set; }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class NativeBroadphase : p2.p22.Broadphase
            {
                public extern NativeBroadphase(double type);

                public new static double AABB { get; set; }

                public new static double BOUNDING_CIRCLE { get; set; }

                public new static double NAIVE { get; set; }

                public new static double SAP { get; set; }

                public new static extern bool boundingRadiusCheck(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public new static extern bool aabbCheck(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public new static extern bool canCollide(p2.p22.Body bodyA, p2.p22.Body bodyB);
            }

            public class Narrowphase : IObject
            {
                public virtual p2.p22.ContactEquation[] contactEquations { get; set; }

                public virtual p2.p22.FrictionEquation[] frictionEquations { get; set; }

                public virtual bool enableFriction { get; set; }

                public virtual bool enableEquations { get; set; }

                public virtual double slipForce { get; set; }

                public virtual double frictionCoefficient { get; set; }

                public virtual double surfaceVelocity { get; set; }

                public virtual bool reuseObjects { get; set; }

                public virtual object[] resuableContactEquations { get; set; }

                public virtual object[] reusableFrictionEquations { get; set; }

                public virtual double restitution { get; set; }

                public virtual double stiffness { get; set; }

                public virtual double relaxation { get; set; }

                public virtual double frictionStiffness { get; set; }

                public virtual double frictionRelaxation { get; set; }

                public virtual bool enableFrictionReduction { get; set; }

                public virtual double contactSkinSize { get; set; }

                public virtual extern bool collidedLastStep(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public virtual extern void reset();

                public virtual extern p2.p22.ContactEquation createContactEquation(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  p2.p22.Shape shapeA,
                  p2.p22.Shape shapeB);

                public virtual extern p2.p22.FrictionEquation createFrictionFromContact(
                  p2.p22.ContactEquation c);

                public Narrowphase() : base()
                {
                    
                }
            }

            public class SAPBroadphase : p2.p22.Broadphase
            {
                public extern SAPBroadphase(double type);

                public virtual p2.p22.Body[] axisList { get; set; }

                public virtual double axisIndex { get; set; }

                public new static double AABB { get; set; }

                public new static double BOUNDING_CIRCLE { get; set; }

                public new static double NAIVE { get; set; }

                public new static double SAP { get; set; }

                public new static extern bool boundingRadiusCheck(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public new static extern bool aabbCheck(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public new static extern bool canCollide(p2.p22.Body bodyA, p2.p22.Body bodyB);
            }

            public class Constraint : IObject
            {
                public static double DISTANCE { get; set; }

                public static double GEAR { get; set; }

                public static double LOCK { get; set; }

                public static double PRISMATIC { get; set; }

                public static double REVOLUTE { get; set; }

                public extern Constraint(p2.p22.Body bodyA, p2.p22.Body bodyB, double type);

                public extern Constraint(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  double type,
                  p2.p22.Constraint.Config options);

                public virtual double type { get; set; }

                public virtual p2.p22.Equation[] equeations { get; set; }

                public virtual p2.p22.Body bodyA { get; set; }

                public virtual p2.p22.Body bodyB { get; set; }

                public virtual bool collideConnected { get; set; }

                public virtual extern void update();

                public virtual extern void setStiffness(double stiffness);

                public virtual extern void setRelaxation(double relaxation);

                [ObjectLiteral]
                public class Config : IObject
                {
                    public bool? collideConnected { get; set; }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class DistanceConstraint : p2.p22.Constraint
            {
                public extern DistanceConstraint(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern DistanceConstraint(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  p2.p22.DistanceConstraint.Config options);

                public virtual double[] localAnchorA { get; set; }

                public virtual double[] localAnchorB { get; set; }

                public virtual double distance { get; set; }

                public virtual double maxForce { get; set; }

                public virtual bool upperLimitEnabled { get; set; }

                public virtual double upperLimit { get; set; }

                public virtual bool lowerLimitEnabled { get; set; }

                public virtual double lowerLimit { get; set; }

                public virtual double position { get; set; }

                public virtual extern void setMaxForce(double f);

                public virtual extern double getMaxForce();

                public new static double DISTANCE { get; set; }

                public new static double GEAR { get; set; }

                public new static double LOCK { get; set; }

                public new static double PRISMATIC { get; set; }

                public new static double REVOLUTE { get; set; }

                [ObjectLiteral]
                public new class Config : IObject
                {
                    public double? distance { get; set; }

                    public double[] localAnchorA { get; set; }

                    public double[] localAnchorB { get; set; }

                    public double? maxForce { get; set; }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class GearConstraint : p2.p22.Constraint
            {
                public extern GearConstraint(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern GearConstraint(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  p2.p22.GearConstraint.Config options);

                public virtual double ratio { get; set; }

                public virtual double angle { get; set; }

                public virtual extern void setMaxTorque(double torque);

                public virtual extern double getMaxTorque();

                public new static double DISTANCE { get; set; }

                public new static double GEAR { get; set; }

                public new static double LOCK { get; set; }

                public new static double PRISMATIC { get; set; }

                public new static double REVOLUTE { get; set; }

                [ObjectLiteral]
                public new class Config : IObject
                {
                    public double? angle { get; set; }

                    public double? ratio { get; set; }

                    public double? maxTorque { get; set; }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class LockConstraint : p2.p22.Constraint
            {
                public extern LockConstraint(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern LockConstraint(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  p2.p22.LockConstraint.Config options);

                public virtual extern void setMaxForce(double force);

                public virtual extern double getMaxForce();

                public new static double DISTANCE { get; set; }

                public new static double GEAR { get; set; }

                public new static double LOCK { get; set; }

                public new static double PRISMATIC { get; set; }

                public new static double REVOLUTE { get; set; }

                [ObjectLiteral]
                public new class Config : IObject
                {
                    public double[] localOffsetB { get; set; }

                    public double? localAngleB { get; set; }

                    public double? maxForce { get; set; }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class PrismaticConstraint : p2.p22.Constraint
            {
                public extern PrismaticConstraint(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern PrismaticConstraint(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  p2.p22.PrismaticConstraint.Config options);

                public virtual double[] localAnchorA { get; set; }

                public virtual double[] localAnchorB { get; set; }

                public virtual double[] localAxisA { get; set; }

                public virtual double position { get; set; }

                public virtual double velocity { get; set; }

                public virtual bool lowerLimitEnabled { get; set; }

                public virtual bool upperLimitEnabled { get; set; }

                public virtual double lowerLimit { get; set; }

                public virtual double upperLimit { get; set; }

                public virtual p2.p22.ContactEquation upperLimitEquation { get; set; }

                public virtual p2.p22.ContactEquation lowerLimitEquation { get; set; }

                public virtual p2.p22.Equation motorEquation { get; set; }

                public virtual bool motorEnabled { get; set; }

                public virtual double motorSpeed { get; set; }

                public virtual extern void enableMotor();

                public virtual extern void disableMotor();

                public virtual extern void setLimits(double lower, double upper);

                public new static double DISTANCE { get; set; }

                public new static double GEAR { get; set; }

                public new static double LOCK { get; set; }

                public new static double PRISMATIC { get; set; }

                public new static double REVOLUTE { get; set; }

                [ObjectLiteral]
                public new class Config : IObject
                {
                    public double? maxForce { get; set; }

                    public double[] localAnchorA { get; set; }

                    public double[] localAnchorB { get; set; }

                    public double[] localAxisA { get; set; }

                    public bool? disableRotationalLock { get; set; }

                    public double? upperLimit { get; set; }

                    public double? lowerLimit { get; set; }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class RevoluteConstraint : p2.p22.Constraint
            {
                public extern RevoluteConstraint(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern RevoluteConstraint(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  p2.p22.RevoluteConstraint.Config options);

                public virtual double[] pivotA { get; set; }

                public virtual double[] pivotB { get; set; }

                public virtual p2.p22.RotationalVelocityEquation motorEquation { get; set; }

                public virtual bool motorEnabled { get; set; }

                public virtual double angle { get; set; }

                public virtual bool lowerLimitEnabled { get; set; }

                public virtual bool upperLimitEnabled { get; set; }

                public virtual double lowerLimit { get; set; }

                public virtual double upperLimit { get; set; }

                public virtual p2.p22.ContactEquation upperLimitEquation { get; set; }

                public virtual p2.p22.ContactEquation lowerLimitEquation { get; set; }

                public virtual extern void enableMotor();

                public virtual extern void disableMotor();

                public virtual extern bool motorIsEnabled();

                public virtual extern void setLimits(double lower, double upper);

                public virtual extern void setMotorSpeed(double speed);

                public virtual extern double getMotorSpeed();

                public new static double DISTANCE { get; set; }

                public new static double GEAR { get; set; }

                public new static double LOCK { get; set; }

                public new static double PRISMATIC { get; set; }

                public new static double REVOLUTE { get; set; }

                [ObjectLiteral]
                public new class Config : IObject
                {
                    public double[] worldPivot { get; set; }

                    public double[] localPivotA { get; set; }

                    public double[] localPivotB { get; set; }

                    public double? maxForce { get; set; }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class AngleLockEquation : p2.p22.Equation
            {
                public extern AngleLockEquation(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern AngleLockEquation(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  p2.p22.AngleLockEquation.Config options);

                public override extern double computeGq();

                public virtual extern double setRatio(double ratio);

                public virtual extern double setMaxTorque(double torque);

                public new static double DEFAULT_STIFFNESS { get; set; }

                public new static double DEFAULT_RELAXATION { get; set; }

                [ObjectLiteral]
                public class Config : IObject
                {
                    public double? angle { get; set; }

                    public double? ratio { get; set; }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class ContactEquation : p2.p22.Equation
            {
                public extern ContactEquation(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public virtual double[] contactPointA { get; set; }

                public virtual double[] penetrationVec { get; set; }

                public virtual double[] contactPointB { get; set; }

                public virtual double[] normalA { get; set; }

                public virtual double restitution { get; set; }

                public virtual bool firstImpact { get; set; }

                public virtual p2.p22.Shape shapeA { get; set; }

                public virtual p2.p22.Shape shapeB { get; set; }

                public override extern double computeB(double a, double b, double h);

                public new static double DEFAULT_STIFFNESS { get; set; }

                public new static double DEFAULT_RELAXATION { get; set; }
            }

            public class Equation : IObject
            {
                public static double DEFAULT_STIFFNESS { get; set; }

                public static double DEFAULT_RELAXATION { get; set; }

                public extern Equation(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern Equation(p2.p22.Body bodyA, p2.p22.Body bodyB, double minForce);

                public extern Equation(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  double minForce,
                  double maxForce);

                public virtual double minForce { get; set; }

                public virtual double maxForce { get; set; }

                public virtual p2.p22.Body bodyA { get; set; }

                public virtual p2.p22.Body bodyB { get; set; }

                public virtual double stiffness { get; set; }

                public virtual double relaxation { get; set; }

                public virtual double[] G { get; set; }

                public virtual double offset { get; set; }

                public virtual double a { get; set; }

                public virtual double b { get; set; }

                public virtual double epsilon { get; set; }

                public virtual double timeStep { get; set; }

                public virtual bool needsUpdate { get; set; }

                public virtual double multiplier { get; set; }

                public virtual double relativeVelocity { get; set; }

                public virtual bool enabled { get; set; }

                public virtual extern double gmult(
                  double[] G,
                  double[] vi,
                  double[] wi,
                  double[] vj,
                  double[] wj);

                public virtual extern double computeB(double a, double b, double h);

                public virtual extern double computeGq();

                public virtual extern double computeGW();

                public virtual extern double computeGWlambda();

                public virtual extern double computeGiMf();

                public virtual extern double computeGiMGt();

                public virtual extern double addToWlambda(double deltalambda);

                public virtual extern double computeInvC(double eps);
            }

            public class FrictionEquation : p2.p22.Equation
            {
                public extern FrictionEquation(p2.p22.Body bodyA, p2.p22.Body bodyB, double slipForce);

                public virtual double[] contactPointA { get; set; }

                public virtual double[] contactPointB { get; set; }

                public virtual double[] t { get; set; }

                public virtual p2.p22.Shape shapeA { get; set; }

                public virtual p2.p22.Shape shapeB { get; set; }

                public virtual double frictionCoefficient { get; set; }

                public virtual extern double setSlipForce(double slipForce);

                public virtual extern double getSlipForce();

                public override extern double computeB(double a, double b, double h);

                public new static double DEFAULT_STIFFNESS { get; set; }

                public new static double DEFAULT_RELAXATION { get; set; }
            }

            public class RotationalLockEquation : p2.p22.Equation
            {
                public extern RotationalLockEquation(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern RotationalLockEquation(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  p2.p22.RotationalLockEquation.Config options);

                public virtual double angle { get; set; }

                public override extern double computeGq();

                public new static double DEFAULT_STIFFNESS { get; set; }

                public new static double DEFAULT_RELAXATION { get; set; }

                [ObjectLiteral]
                public class Config : IObject
                {
                    public double? angle { get; set; }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class RotationalVelocityEquation : p2.p22.Equation
            {
                public extern RotationalVelocityEquation(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public override extern double computeB(double a, double b, double h);

                public new static double DEFAULT_STIFFNESS { get; set; }

                public new static double DEFAULT_RELAXATION { get; set; }
            }

            public class EventEmitter : IObject
            {
                public virtual extern p2.p22.EventEmitter on(
                  string type,
                  es5.Function listener,
                  object context);

                public virtual extern p2.p22.EventEmitter on(
                  string type,
                  Action listener,
                  object context);

                public virtual extern p2.p22.EventEmitter on(
                  string type,
                  Func<object> listener,
                  object context);

                public virtual extern bool has(string type, es5.Function listener);

                public virtual extern bool has(string type, Action listener);

                public virtual extern bool has(string type, Func<object> listener);

                public virtual extern p2.p22.EventEmitter off(string type, es5.Function listener);

                public virtual extern p2.p22.EventEmitter off(string type, Action listener);

                public virtual extern p2.p22.EventEmitter off(string type, Func<object> listener);

                public virtual extern p2.p22.EventEmitter emit(object @event);

                public EventEmitter() : base()
                {
                    
                }
            }

            public class ContactMaterialOptions : IObject
            {
                public virtual double? friction { get; set; }

                public virtual double? restitution { get; set; }

                public virtual double? stiffness { get; set; }

                public virtual double? relaxation { get; set; }

                public virtual double? frictionStiffness { get; set; }

                public virtual double? frictionRelaxation { get; set; }

                public virtual double? surfaceVelocity { get; set; }

                public ContactMaterialOptions() : base()
                {
                    
                }
            }

            public class ContactMaterial : IObject
            {
                public static double idCounter { get; set; }

                public extern ContactMaterial(p2.p22.Material materialA, p2.p22.Material materialB);

                public extern ContactMaterial(
                  p2.p22.Material materialA,
                  p2.p22.Material materialB,
                  p2.p22.ContactMaterialOptions options);

                public virtual double id { get; set; }

                public virtual p2.p22.Material materialA { get; set; }

                public virtual p2.p22.Material materialB { get; set; }

                public virtual double friction { get; set; }

                public virtual double restitution { get; set; }

                public virtual double stiffness { get; set; }

                public virtual double relaxation { get; set; }

                public virtual double frictionStiffness { get; set; }

                public virtual double frictionRelaxation { get; set; }

                public virtual double surfaceVelocity { get; set; }

                public virtual double contactSkinSize { get; set; }
            }

            public class Material : IObject
            {
                public static double idCounter { get; set; }

                public extern Material(double id);

                public virtual double id { get; set; }
            }

            public class vec2 : IObject
            {
                public static extern double crossLength(double[] a, double[] b);

                public static extern double crossVZ(double[] @out, double[] vec, double zcomp);

                public static extern double crossZV(double[] @out, double zcomp, double[] vec);

                public static extern void rotate(double[] @out, double[] a, double angle);

                public static extern double rotate90cw(double[] @out, double[] a);

                public static extern double[] centroid(double[] @out, double[] a, double[] b, double[] c);

                public static extern double[] create();

                public static extern double[] clone(double[] a);

                public static extern double[] fromValues(double x, double y);

                public static extern double[] copy(double[] @out, double[] a);

                public static extern double[] set(double[] @out, double x, double y);

                public static extern void toLocalFrame(
                  double[] @out,
                  double[] worldPoint,
                  double[] framePosition,
                  double frameAngle);

                public static extern void toGlobalFrame(
                  double[] @out,
                  double[] localPoint,
                  double[] framePosition,
                  double frameAngle);

                public static extern double[] add(double[] @out, double[] a, double[] b);

                public static extern double[] subtract(double[] @out, double[] a, double[] b);

                public static extern double[] sub(double[] @out, double[] a, double[] b);

                public static extern double[] multiply(double[] @out, double[] a, double[] b);

                public static extern double[] mul(double[] @out, double[] a, double[] b);

                public static extern double[] divide(double[] @out, double[] a, double[] b);

                public static extern double[] div(double[] @out, double[] a, double[] b);

                public static extern double[] scale(double[] @out, double[] a, double b);

                public static extern double distance(double[] a, double[] b);

                public static extern double dist(double[] a, double[] b);

                public static extern double squaredDistance(double[] a, double[] b);

                public static extern double sqrDist(double[] a, double[] b);

                public static extern double length(double[] a);

                public static extern double len(double[] a);

                public static extern double squaredLength(double[] a);

                public static extern double sqrLen(double[] a);

                public static extern double[] negate(double[] @out, double[] a);

                public static extern double[] normalize(double[] @out, double[] a);

                public static extern double dot(double[] a, double[] b);

                public static extern string str(double[] a);

                public vec2() : base()
                {
                    
                }
            }

            [IgnoreCast]
            [ObjectLiteral]
            [FormerInterface]
            public class BodyOptions : IObject
            {
                public double? mass { get; set; }

                public double[] position { get; set; }

                public double[] velocity { get; set; }

                public double? angle { get; set; }

                public double? angularVelocity { get; set; }

                public double[] force { get; set; }

                public double? angularForce { get; set; }

                public bool? fixedRotation { get; set; }

                public BodyOptions() : base()
                {
                    
                }
            }

            public class Body : p2.p22.EventEmitter
            {
                public virtual p2.p22.Body.sleepyEventConfig sleepyEvent { get; set; }

                public virtual p2.p22.Body.sleepEventConfig sleepEvent { get; set; }

                public virtual p2.p22.Body.wakeUpEventConfig wakeUpEvent { get; set; }

                public static double DYNAMIC { get; set; }

                public static double STATIC { get; set; }

                public static double KINEMATIC { get; set; }

                public static double AWAKE { get; set; }

                public static double SLEEPY { get; set; }

                public static double SLEEPING { get; set; }

                public extern Body();

                public extern Body(p2.p22.BodyOptions options);

                public virtual double id { get; set; }

                public virtual p2.p22.World world { get; set; }

                public virtual p2.p22.Shape[] shapes { get; set; }

                public virtual double mass { get; set; }

                public virtual double invMass { get; set; }

                public virtual double inertia { get; set; }

                public virtual double invInertia { get; set; }

                public virtual double invMassSolve { get; set; }

                public virtual double invInertiaSolve { get; set; }

                public virtual double fixedRotation { get; set; }

                public virtual double[] position { get; set; }

                public virtual double[] interpolatedPosition { get; set; }

                public virtual double interpolatedAngle { get; set; }

                public virtual double[] previousPosition { get; set; }

                public virtual double previousAngle { get; set; }

                public virtual double[] velocity { get; set; }

                public virtual double[] vlambda { get; set; }

                public virtual double[] wlambda { get; set; }

                public virtual double angle { get; set; }

                public virtual double angularVelocity { get; set; }

                public virtual double[] force { get; set; }

                public virtual double angularForce { get; set; }

                public virtual double damping { get; set; }

                public virtual double angularDamping { get; set; }

                public virtual double type { get; set; }

                public virtual double boundingRadius { get; set; }

                public virtual p2.p22.AABB aabb { get; set; }

                public virtual bool aabbNeedsUpdate { get; set; }

                public virtual bool allowSleep { get; set; }

                public virtual bool wantsToSleep { get; set; }

                public virtual double sleepState { get; set; }

                public virtual double sleepSpeedLimit { get; set; }

                public virtual double sleepTimeLimit { get; set; }

                public virtual double gravityScale { get; set; }

                public virtual bool collisionResponse { get; set; }

                public virtual extern void updateSolveMassProperties();

                public virtual extern void setDensity(double density);

                public virtual extern double getArea();

                public virtual extern p2.p22.AABB getAABB();

                public virtual extern void updateAABB();

                public virtual extern void updateBoundingRadius();

                public virtual extern void addShape(p2.p22.Shape shape);

                public virtual extern void addShape(p2.p22.Shape shape, double[] offset);

                public virtual extern void addShape(p2.p22.Shape shape, double[] offset, double angle);

                public virtual extern bool removeShape(p2.p22.Shape shape);

                public virtual extern void updateMassProperties();

                public virtual extern void applyForce(double[] force);

                public virtual extern void applyForce(double[] force, double[] relativePoint);

                public virtual extern void applyForceLocal(double[] localforce);

                public virtual extern void applyForceLocal(double[] localforce, double[] localPoint);

                public virtual extern void applyImpulse(double[] impulse);

                public virtual extern void applyImpulse(double[] impulse, double[] relativePoint);

                public virtual extern void applyImpulseLocal(double[] impulse);

                public virtual extern void applyImpulseLocal(double[] impulse, double[] localPoint);

                public virtual extern void toLocalFrame(double[] @out, double[] worldPoint);

                public virtual extern void toWorldFrame(double[] @out, double[] localPoint);

                public virtual extern bool fromPolygon(double[][] path);

                public virtual extern bool fromPolygon(
                  double[][] path,
                  p2.p22.Body.fromPolygonConfig options);

                public virtual extern void adjustCenterOfMass();

                public virtual extern void setZeroForce();

                public virtual extern void resetConstraintVelocity();

                public virtual extern void applyDamping(double dy);

                public virtual extern void wakeUp();

                public virtual extern void sleep();

                public virtual extern void sleepTick(double time, bool dontSleep, double dt);

                public virtual extern double[] getVelocityFromPosition(double[] story, double dt);

                public virtual extern double getAngularVelocityFromPosition(double timeStep);

                public virtual extern bool overlaps(p2.p22.Body body);

                [ObjectLiteral]
                public class sleepyEventConfig : IObject
                {
                    public string type { get; set; }

                    public static extern implicit operator p2.p22.Body.sleepyEventConfig(
                      p2.p22.Body.sleepEventConfig value);

                    public static extern implicit operator p2.p22.Body.sleepyEventConfig(
                      p2.p22.Body.wakeUpEventConfig value);

                    public sleepyEventConfig() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class sleepEventConfig : IObject
                {
                    public string type { get; set; }

                    public static extern implicit operator p2.p22.Body.sleepEventConfig(
                      p2.p22.Body.sleepyEventConfig value);

                    public static extern implicit operator p2.p22.Body.sleepEventConfig(
                      p2.p22.Body.wakeUpEventConfig value);

                    public sleepEventConfig() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class wakeUpEventConfig : IObject
                {
                    public string type { get; set; }

                    public static extern implicit operator p2.p22.Body.wakeUpEventConfig(
                      p2.p22.Body.sleepyEventConfig value);

                    public static extern implicit operator p2.p22.Body.wakeUpEventConfig(
                      p2.p22.Body.sleepEventConfig value);

                    public wakeUpEventConfig() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class fromPolygonConfig : IObject
                {
                    public bool? optimalDecomp { get; set; }

                    public bool? skipSimpleCheck { get; set; }

                    public object removeCollinearPoints { get; set; }

                    public fromPolygonConfig() : base()
                    {
                        
                    }
                }
            }

            public class Spring : IObject
            {
                public extern Spring(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern Spring(p2.p22.Body bodyA, p2.p22.Body bodyB, p2.p22.Spring.Config options);

                public virtual double stiffness { get; set; }

                public virtual double damping { get; set; }

                public virtual p2.p22.Body bodyA { get; set; }

                public virtual p2.p22.Body bodyB { get; set; }

                public virtual extern void applyForce();

                [ObjectLiteral]
                public class Config : IObject
                {
                    public double? stiffness { get; set; }

                    public double? damping { get; set; }

                    public double[] localAnchorA { get; set; }

                    public double[] localAnchorB { get; set; }

                    public double[] worldAnchorA { get; set; }

                    public double[] worldAnchorB { get; set; }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class LinearSpring : p2.p22.Spring
            {
                public extern LinearSpring(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public virtual double[] localAnchorA { get; set; }

                public virtual double[] localAnchorB { get; set; }

                public virtual double restLength { get; set; }

                public virtual extern void setWorldAnchorA(double[] worldAnchorA);

                public virtual extern void setWorldAnchorB(double[] worldAnchorB);

                public virtual extern double[] getWorldAnchorA(double[] result);

                public virtual extern double[] getWorldAnchorB(double[] result);

                public override extern void applyForce();
            }

            public class RotationalSpring : p2.p22.Spring
            {
                public extern RotationalSpring(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern RotationalSpring(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  p2.p22.RotationalSpring.Config options);

                public virtual double restAngle { get; set; }

                [ObjectLiteral]
                public new class Config : IObject
                {
                    public double? restAngle { get; set; }

                    public double? stiffness { get; set; }

                    public double? damping { get; set; }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            [IgnoreCast]
            [ObjectLiteral]
            [FormerInterface]
            public class CapsuleOptions :
            /*[IgnoreCast]*/
            /*[ObjectLiteral]*/
            /*[FormerInterface]*/
            p2.p22.SharedShapeOptions
            {
                public double? length { get; set; }

                public double? radius { get; set; }

                public CapsuleOptions() : base()
                {
                    
                }
            }

            public class Capsule : p2.p22.Shape
            {
                public extern Capsule();

                public extern Capsule(p2.p22.CapsuleOptions options);

                public virtual double length { get; set; }

                public virtual double radius { get; set; }

                public new static double idCounter { get; set; }

                public new static double CIRCLE { get; set; }

                public new static double PARTICLE { get; set; }

                public new static double PLANE { get; set; }

                public new static double CONVEX { get; set; }

                public new static double LINE { get; set; }

                public new static double BOX { get; set; }

                public new static double CAPSULE { get; set; }

                public new static double HEIGHTFIELD { get; set; }
            }

            [IgnoreCast]
            [ObjectLiteral]
            [FormerInterface]
            public class CircleOptions :
            /*[IgnoreCast]*/
            /*[ObjectLiteral]*/
            /*[FormerInterface]*/
            p2.p22.SharedShapeOptions
            {
                public double? radius { get; set; }

                public CircleOptions() : base()
                {
                    
                }
            }

            public class Circle : p2.p22.Shape
            {
                public extern Circle();

                public extern Circle(p2.p22.CircleOptions options);

                public virtual double radius { get; set; }

                public new static double idCounter { get; set; }

                public new static double CIRCLE { get; set; }

                public new static double PARTICLE { get; set; }

                public new static double PLANE { get; set; }

                public new static double CONVEX { get; set; }

                public new static double LINE { get; set; }

                public new static double BOX { get; set; }

                public new static double CAPSULE { get; set; }

                public new static double HEIGHTFIELD { get; set; }
            }

            [IgnoreCast]
            [ObjectLiteral]
            [FormerInterface]
            public class ConvexOptions :
            /*[IgnoreCast]*/
            /*[ObjectLiteral]*/
            /*[FormerInterface]*/
            p2.p22.SharedShapeOptions
            {
                public es5.ArrayLike<double>[] vertices { get; set; }

                public es5.ArrayLike<double>[] axes { get; set; }

                public ConvexOptions() : base()
                {
                    
                }
            }

            public class Convex : p2.p22.Shape
            {
                public static extern double triangleArea(double[] a, double[] b, double[] c);

                public extern Convex();

                public extern Convex(p2.p22.ConvexOptions options);

                public virtual double[][] vertices { get; set; }

                public virtual double[][] axes { get; set; }

                public virtual double[] centerOfMass { get; set; }

                public virtual double[] triangles { get; set; }

                public override double boundingRadius { get; set; }

                public virtual extern void projectOntoLocalAxis(double[] localAxis, double[] result);

                public virtual extern void projectOntoWorldAxis(
                  double[] localAxis,
                  double[] shapeOffset,
                  double shapeAngle,
                  double[] result);

                public virtual extern void updateCenterOfMass();

                public new static double idCounter { get; set; }

                public new static double CIRCLE { get; set; }

                public new static double PARTICLE { get; set; }

                public new static double PLANE { get; set; }

                public new static double CONVEX { get; set; }

                public new static double LINE { get; set; }

                public new static double BOX { get; set; }

                public new static double CAPSULE { get; set; }

                public new static double HEIGHTFIELD { get; set; }
            }

            [IgnoreCast]
            [ObjectLiteral]
            [FormerInterface]
            public class HeightfieldOptions :
            /*[IgnoreCast]*/
            /*[ObjectLiteral]*/
            /*[FormerInterface]*/
            p2.p22.SharedShapeOptions
            {
                public double[] heights { get; set; }

                public double? minValue { get; set; }

                public double? maxValue { get; set; }

                public double? elementWidth { get; set; }

                public HeightfieldOptions() : base()
                {
                    
                }
            }

            public class Heightfield : p2.p22.Shape
            {
                public extern Heightfield();

                public extern Heightfield(p2.p22.HeightfieldOptions options);

                public virtual double[] data { get; set; }

                public virtual double maxValue { get; set; }

                public virtual double minValue { get; set; }

                public virtual double elementWidth { get; set; }

                public new static double idCounter { get; set; }

                public new static double CIRCLE { get; set; }

                public new static double PARTICLE { get; set; }

                public new static double PLANE { get; set; }

                public new static double CONVEX { get; set; }

                public new static double LINE { get; set; }

                public new static double BOX { get; set; }

                public new static double CAPSULE { get; set; }

                public new static double HEIGHTFIELD { get; set; }
            }

            [IgnoreCast]
            [ObjectLiteral]
            [FormerInterface]
            public class SharedShapeOptions : IObject
            {
                public double[] position { get; set; }

                public double? angle { get; set; }

                public double? collisionGroup { get; set; }

                public bool? collisionResponse { get; set; }

                public double? collisionMask { get; set; }

                public bool? sensor { get; set; }

                public SharedShapeOptions() : base()
                {
                    
                }
            }

            [IgnoreCast]
            [ObjectLiteral]
            [FormerInterface]
            public class ShapeOptions :
            /*[IgnoreCast]*/
            /*[ObjectLiteral]*/
            /*[FormerInterface]*/
            p2.p22.SharedShapeOptions
            {
                public double? type { get; set; }

                public ShapeOptions() : base()
                {
                    
                }
            }

            public class Shape : IObject
            {
                public static double idCounter { get; set; }

                public static double CIRCLE { get; set; }

                public static double PARTICLE { get; set; }

                public static double PLANE { get; set; }

                public static double CONVEX { get; set; }

                public static double LINE { get; set; }

                public static double BOX { get; set; }

                public static double CAPSULE { get; set; }

                public static double HEIGHTFIELD { get; set; }

                public extern Shape();

                public extern Shape(p2.p22.ShapeOptions options);

                public virtual double type { get; set; }

                public virtual double id { get; set; }

                public virtual double[] position { get; set; }

                public virtual double angle { get; set; }

                public virtual double boundingRadius { get; set; }

                public virtual double collisionGroup { get; set; }

                public virtual bool collisionResponse { get; set; }

                public virtual double collisionMask { get; set; }

                public virtual p2.p22.Material material { get; set; }

                public virtual double area { get; set; }

                public virtual bool sensor { get; set; }

                public virtual extern double computeMomentOfInertia(double mass);

                public virtual extern double updateBoundingRadius();

                public virtual extern void updateArea();

                public virtual extern void computeAABB(p2.p22.AABB @out, double[] position, double angle);
            }

            [IgnoreCast]
            [ObjectLiteral]
            [FormerInterface]
            public class LineOptions :
            /*[IgnoreCast]*/
            /*[ObjectLiteral]*/
            /*[FormerInterface]*/
            p2.p22.SharedShapeOptions
            {
                public double? length { get; set; }

                public LineOptions() : base()
                {
                    
                }
            }

            public class Line : p2.p22.Shape
            {
                public extern Line();

                public extern Line(p2.p22.LineOptions options);

                public virtual double length { get; set; }

                public new static double idCounter { get; set; }

                public new static double CIRCLE { get; set; }

                public new static double PARTICLE { get; set; }

                public new static double PLANE { get; set; }

                public new static double CONVEX { get; set; }

                public new static double LINE { get; set; }

                public new static double BOX { get; set; }

                public new static double CAPSULE { get; set; }

                public new static double HEIGHTFIELD { get; set; }
            }

            public class Particle : p2.p22.Shape
            {
                public extern Particle();

                public extern Particle(p2.p22.SharedShapeOptions options);

                public new static double idCounter { get; set; }

                public new static double CIRCLE { get; set; }

                public new static double PARTICLE { get; set; }

                public new static double PLANE { get; set; }

                public new static double CONVEX { get; set; }

                public new static double LINE { get; set; }

                public new static double BOX { get; set; }

                public new static double CAPSULE { get; set; }

                public new static double HEIGHTFIELD { get; set; }
            }

            public class Plane : p2.p22.Shape
            {
                public extern Plane();

                public extern Plane(p2.p22.SharedShapeOptions options);

                public new static double idCounter { get; set; }

                public new static double CIRCLE { get; set; }

                public new static double PARTICLE { get; set; }

                public new static double PLANE { get; set; }

                public new static double CONVEX { get; set; }

                public new static double LINE { get; set; }

                public new static double BOX { get; set; }

                public new static double CAPSULE { get; set; }

                public new static double HEIGHTFIELD { get; set; }
            }

            [IgnoreCast]
            [ObjectLiteral]
            [FormerInterface]
            public class BoxOptions : IObject
            {
                public double? width { get; set; }

                public double? height { get; set; }

                public BoxOptions() : base()
                {
                    
                }
            }

            public class Box : p2.p22.Shape
            {
                public extern Box();

                public extern Box(p2.p22.BoxOptions options);

                public virtual double width { get; set; }

                public virtual double height { get; set; }

                public new static double idCounter { get; set; }

                public new static double CIRCLE { get; set; }

                public new static double PARTICLE { get; set; }

                public new static double PLANE { get; set; }

                public new static double CONVEX { get; set; }

                public new static double LINE { get; set; }

                public new static double BOX { get; set; }

                public new static double CAPSULE { get; set; }

                public new static double HEIGHTFIELD { get; set; }
            }

            public class Solver : p2.p22.EventEmitter
            {
                public static double GS { get; set; }

                public static double ISLAND { get; set; }

                public extern Solver();

                public extern Solver(object options);

                public extern Solver(object options, double type);

                public virtual double type { get; set; }

                public virtual p2.p22.Equation[] equations { get; set; }

                public virtual p2.p22.Equation equationSortFunction { get; set; }

                public virtual extern void solve(double dy, p2.p22.World world);

                public virtual extern void solveIsland(double dy, p2.p22.Island island);

                public virtual extern void sortEquations();

                public virtual extern void addEquation(p2.p22.Equation eq);

                public virtual extern void addEquations(p2.p22.Equation[] eqs);

                public virtual extern void removeEquation(p2.p22.Equation eq);

                public virtual extern void removeAllEquations();
            }

            public class GSSolver : p2.p22.Solver
            {
                public extern GSSolver();

                public extern GSSolver(p2.p22.GSSolver.Config options);

                public virtual double iterations { get; set; }

                public virtual double tolerance { get; set; }

                public virtual bool useZeroRHS { get; set; }

                public virtual double frictionIterations { get; set; }

                public virtual double usedIterations { get; set; }

                public override extern void solve(double h, p2.p22.World world);

                public new static double GS { get; set; }

                public new static double ISLAND { get; set; }

                [ObjectLiteral]
                public class Config : IObject
                {
                    public double? iterations { get; set; }

                    public double? tolerance { get; set; }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class OverlapKeeper : IObject
            {
                public extern OverlapKeeper(
                  p2.p22.Body bodyA,
                  p2.p22.Shape shapeA,
                  p2.p22.Body bodyB,
                  p2.p22.Shape shapeB);

                public virtual p2.p22.Shape shapeA { get; set; }

                public virtual p2.p22.Shape shapeB { get; set; }

                public virtual p2.p22.Body bodyA { get; set; }

                public virtual p2.p22.Body bodyB { get; set; }

                public virtual extern void tick();

                public virtual extern void setOverlapping(
                  p2.p22.Body bodyA,
                  p2.p22.Shape shapeA,
                  p2.p22.Body bodyB,
                  p2.p22.Body shapeB);

                public virtual extern bool bodiesAreOverlapping(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public virtual extern void set(
                  p2.p22.Body bodyA,
                  p2.p22.Shape shapeA,
                  p2.p22.Body bodyB,
                  p2.p22.Shape shapeB);
            }

            public class TupleDictionary : IObject
            {
                public virtual double[] data { get; set; }

                public virtual double[] keys { get; set; }

                public virtual extern string getKey(double id1, double id2);

                public virtual extern double getByKey(double key);

                public virtual extern double get(double i, double j);

                public virtual extern double set(double i, double j, double value);

                public virtual extern void reset();

                public virtual extern void copy(p2.p22.TupleDictionary dict);

                public TupleDictionary() : base()
                {
                    
                }
            }

            public class Utils : IObject
            {
                public static extern es5.Array<T> appendArray<T>(es5.Array<T> a, es5.Array<T> b);

                public static extern void splice<T>(es5.Array<T> array, double index, double howMany);

                public static extern void extend(object a, object b);

                public static extern object defaults(object options, object defaults);

                public Utils() : base()
                {
                    
                }
            }

            public class Island : IObject
            {
                public virtual p2.p22.Equation[] equations { get; set; }

                public virtual p2.p22.Body[] bodies { get; set; }

                public virtual extern void reset();

                public virtual extern p2.p22.Body[] getBodies(object result);

                public virtual extern bool wantsToSleep();

                public virtual extern bool sleep();

                public Island() : base()
                {
                    
                }
            }

            public class IslandManager : p2.p22.Solver
            {
                public static extern p2.p22.IslandNode getUnvisitedNode(p2.p22.IslandNode[] nodes);

                public override p2.p22.Equation[] equations { get; set; }

                public virtual p2.p22.Island[] islands { get; set; }

                public virtual p2.p22.IslandNode[] nodes { get; set; }

                public virtual extern void visit(
                  p2.p22.IslandNode node,
                  p2.p22.Body[] bds,
                  p2.p22.Equation[] eqs);

                public virtual extern void bfs(
                  p2.p22.IslandNode root,
                  p2.p22.Body[] bds,
                  p2.p22.Equation[] eqs);

                public virtual extern p2.p22.Island[] split(p2.p22.World world);

                public new static double GS { get; set; }

                public new static double ISLAND { get; set; }

                public IslandManager() : base()
                {
                    
                }
            }

            public class IslandNode : IObject
            {
                public extern IslandNode(p2.p22.Body body);

                public virtual p2.p22.Body body { get; set; }

                public virtual p2.p22.IslandNode[] neighbors { get; set; }

                public virtual p2.p22.Equation[] equations { get; set; }

                public virtual bool visited { get; set; }

                public virtual extern void reset();
            }

            public class World : p2.p22.EventEmitter
            {
                public virtual p2.p22.World.postStepEventConfig postStepEvent { get; set; }

                public virtual p2.p22.World.addBodyEventConfig addBodyEvent { get; set; }

                public virtual p2.p22.World.removeBodyEventConfig removeBodyEvent { get; set; }

                public virtual p2.p22.World.addSpringEventConfig addSpringEvent { get; set; }

                public virtual p2.p22.World.impactEventConfig impactEvent { get; set; }

                public virtual p2.p22.World.postBroadphaseEventConfig postBroadphaseEvent { get; set; }

                public virtual p2.p22.World.beginContactEventConfig beginContactEvent { get; set; }

                public virtual p2.p22.World.endContactEventConfig endContactEvent { get; set; }

                public virtual p2.p22.World.preSolveEventConfig preSolveEvent { get; set; }

                public static double NO_SLEEPING { get; set; }

                public static double BODY_SLEEPING { get; set; }

                public static double ISLAND_SLEEPING { get; set; }

                public static extern void integrateBody(p2.p22.Body body, double dy);

                public extern World();

                public extern World(p2.p22.World.Config options);

                public virtual p2.p22.Spring[] springs { get; set; }

                public virtual p2.p22.Body[] bodies { get; set; }

                public virtual p2.p22.Solver solver { get; set; }

                public virtual p2.p22.Narrowphase narrowphase { get; set; }

                public virtual p2.p22.IslandManager islandManager { get; set; }

                public virtual double[] gravity { get; set; }

                public virtual double frictionGravity { get; set; }

                public virtual bool useWorldGravityAsFrictionGravity { get; set; }

                public virtual bool useFrictionGravityOnZeroGravity { get; set; }

                public virtual bool doProfiling { get; set; }

                public virtual double lastStepTime { get; set; }

                public virtual p2.p22.Broadphase broadphase { get; set; }

                public virtual p2.p22.Constraint[] constraints { get; set; }

                public virtual p2.p22.Material defaultMaterial { get; set; }

                public virtual p2.p22.ContactMaterial defaultContactMaterial { get; set; }

                public virtual double lastTimeStep { get; set; }

                public virtual bool applySpringForces { get; set; }

                public virtual bool applyDamping { get; set; }

                public virtual bool applyGravity { get; set; }

                public virtual bool solveConstraints { get; set; }

                public virtual p2.p22.ContactMaterial[] contactMaterials { get; set; }

                public virtual double time { get; set; }

                public virtual bool stepping { get; set; }

                public virtual bool islandSplit { get; set; }

                public virtual bool emitImpactEvent { get; set; }

                public virtual double sleepMode { get; set; }

                public virtual extern void addConstraint(p2.p22.Constraint c);

                public virtual extern void addContactMaterial(p2.p22.ContactMaterial contactMaterial);

                public virtual extern void removeContactMaterial(p2.p22.ContactMaterial cm);

                public virtual extern p2.p22.ContactMaterial getContactMaterial(
                  p2.p22.Material materialA,
                  p2.p22.Material materialB);

                public virtual extern void removeConstraint(p2.p22.Constraint c);

                public virtual extern void step(double dy);

                public virtual extern void step(double dy, double timeSinceLastCalled);

                public virtual extern void step(double dy, double timeSinceLastCalled, double maxSubSteps);

                public virtual extern void runNarrowphase(
                  p2.p22.Narrowphase np,
                  p2.p22.Body bi,
                  p2.p22.Shape si,
                  object[] xi,
                  double ai,
                  p2.p22.Body bj,
                  p2.p22.Shape sj,
                  object[] xj,
                  double aj,
                  double cm,
                  double glen);

                public virtual extern void addSpring(p2.p22.Spring s);

                public virtual extern void removeSpring(p2.p22.Spring s);

                public virtual extern void addBody(p2.p22.Body body);

                public virtual extern void removeBody(p2.p22.Body body);

                public virtual extern p2.p22.Body getBodyByID(double id);

                public virtual extern void disableBodyCollision(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public virtual extern void enableBodyCollision(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public virtual extern void clear();

                public virtual extern p2.p22.World clone();

                public virtual extern p2.p22.Body[] hitTest(
                  double[] worldPoint,
                  p2.p22.Body[] bodies,
                  double precision);

                public virtual extern void setGlobalEquationParameters(
                  p2.p22.World.setGlobalEquationParametersConfig parameters);

                public virtual extern void setGlobalStiffness(double stiffness);

                public virtual extern void setGlobalRelaxation(double relaxation);

                [ObjectLiteral]
                public class postStepEventConfig : IObject
                {
                    public string type { get; set; }

                    public static extern implicit operator p2.p22.World.postStepEventConfig(
                      p2.p22.World.addBodyEventConfig value);

                    public static extern implicit operator p2.p22.World.postStepEventConfig(
                      p2.p22.World.removeBodyEventConfig value);

                    public static extern implicit operator p2.p22.World.postStepEventConfig(
                      p2.p22.World.addSpringEventConfig value);

                    public postStepEventConfig() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class addBodyEventConfig : IObject
                {
                    public string type { get; set; }

                    public static extern implicit operator p2.p22.World.addBodyEventConfig(
                      p2.p22.World.postStepEventConfig value);

                    public static extern implicit operator p2.p22.World.addBodyEventConfig(
                      p2.p22.World.removeBodyEventConfig value);

                    public static extern implicit operator p2.p22.World.addBodyEventConfig(
                      p2.p22.World.addSpringEventConfig value);

                    public addBodyEventConfig() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class removeBodyEventConfig : IObject
                {
                    public string type { get; set; }

                    public static extern implicit operator p2.p22.World.removeBodyEventConfig(
                      p2.p22.World.postStepEventConfig value);

                    public static extern implicit operator p2.p22.World.removeBodyEventConfig(
                      p2.p22.World.addBodyEventConfig value);

                    public static extern implicit operator p2.p22.World.removeBodyEventConfig(
                      p2.p22.World.addSpringEventConfig value);

                    public removeBodyEventConfig() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class addSpringEventConfig : IObject
                {
                    public string type { get; set; }

                    public static extern implicit operator p2.p22.World.addSpringEventConfig(
                      p2.p22.World.postStepEventConfig value);

                    public static extern implicit operator p2.p22.World.addSpringEventConfig(
                      p2.p22.World.addBodyEventConfig value);

                    public static extern implicit operator p2.p22.World.addSpringEventConfig(
                      p2.p22.World.removeBodyEventConfig value);

                    public addSpringEventConfig() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class impactEventConfig : IObject
                {
                    public string type { get; set; }

                    public p2.p22.Body bodyA { get; set; }

                    public p2.p22.Body bodyB { get; set; }

                    public p2.p22.Shape shapeA { get; set; }

                    public p2.p22.Shape shapeB { get; set; }

                    public p2.p22.ContactEquation contactEquation { get; set; }

                    public impactEventConfig() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class postBroadphaseEventConfig : IObject
                {
                    public string type { get; set; }

                    public p2.p22.Body[] pairs { get; set; }

                    public postBroadphaseEventConfig() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class beginContactEventConfig : IObject
                {
                    public string type { get; set; }

                    public p2.p22.Shape shapeA { get; set; }

                    public p2.p22.Shape shapeB { get; set; }

                    public p2.p22.Body bodyA { get; set; }

                    public p2.p22.Body bodyB { get; set; }

                    public p2.p22.ContactEquation[] contactEquations { get; set; }

                    public beginContactEventConfig() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class endContactEventConfig : IObject
                {
                    public string type { get; set; }

                    public p2.p22.Shape shapeA { get; set; }

                    public p2.p22.Shape shapeB { get; set; }

                    public p2.p22.Body bodyA { get; set; }

                    public p2.p22.Body bodyB { get; set; }

                    public endContactEventConfig() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class preSolveEventConfig : IObject
                {
                    public string type { get; set; }

                    public p2.p22.ContactEquation[] contactEquations { get; set; }

                    public p2.p22.FrictionEquation[] frictionEquations { get; set; }

                    public preSolveEventConfig() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class Config : IObject
                {
                    public p2.p22.Solver solver { get; set; }

                    public double[] gravity { get; set; }

                    public p2.p22.Broadphase broadphase { get; set; }

                    public bool? islandSplit { get; set; }

                    public bool? doProfiling { get; set; }

                    public Config() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class setGlobalEquationParametersConfig : IObject
                {
                    public double? relaxation { get; set; }

                    public double? stiffness { get; set; }

                    public setGlobalEquationParametersConfig() : base()
                    {
                        
                    }
                }
            }
        }
    }
}
