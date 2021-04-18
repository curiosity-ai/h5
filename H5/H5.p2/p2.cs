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
                    private double[] upperBoundk__BackingField;
                    private double[] lowerBoundk__BackingField;

                    public double[] upperBound
                    {
                        get
                        {
                            return this.upperBoundk__BackingField;
                        }
                        set
                        {
                            this.upperBoundk__BackingField = value;
                        }
                    }

                    public double[] lowerBound
                    {
                        get
                        {
                            return this.lowerBoundk__BackingField;
                        }
                        set
                        {
                            this.lowerBoundk__BackingField = value;
                        }
                    }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class Broadphase : IObject
            {
                private static double AABBk__BackingField;
                private static double BOUNDING_CIRCLEk__BackingField;
                private static double NAIVEk__BackingField;
                private static double SAPk__BackingField;
                private double typek__BackingField;
                private p2.p22.Body[] resultk__BackingField;
                private p2.p22.World worldk__BackingField;
                private double boundingVolumeTypek__BackingField;

                public static double AABB
                {
                    get
                    {
                        return p2.p22.Broadphase.AABBk__BackingField;
                    }
                    set
                    {
                        p2.p22.Broadphase.AABBk__BackingField = value;
                    }
                }

                public static double BOUNDING_CIRCLE
                {
                    get
                    {
                        return p2.p22.Broadphase.BOUNDING_CIRCLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Broadphase.BOUNDING_CIRCLEk__BackingField = value;
                    }
                }

                public static double NAIVE
                {
                    get
                    {
                        return p2.p22.Broadphase.NAIVEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Broadphase.NAIVEk__BackingField = value;
                    }
                }

                public static double SAP
                {
                    get
                    {
                        return p2.p22.Broadphase.SAPk__BackingField;
                    }
                    set
                    {
                        p2.p22.Broadphase.SAPk__BackingField = value;
                    }
                }

                public static extern bool boundingRadiusCheck(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public static extern bool aabbCheck(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public static extern bool canCollide(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern Broadphase(double type);

                public virtual double type
                {
                    get
                    {
                        return this.typek__BackingField;
                    }
                    set
                    {
                        this.typek__BackingField = value;
                    }
                }

                public virtual p2.p22.Body[] result
                {
                    get
                    {
                        return this.resultk__BackingField;
                    }
                    set
                    {
                        this.resultk__BackingField = value;
                    }
                }

                public virtual p2.p22.World world
                {
                    get
                    {
                        return this.worldk__BackingField;
                    }
                    set
                    {
                        this.worldk__BackingField = value;
                    }
                }

                public virtual double boundingVolumeType
                {
                    get
                    {
                        return this.boundingVolumeTypek__BackingField;
                    }
                    set
                    {
                        this.boundingVolumeTypek__BackingField = value;
                    }
                }

                public virtual extern void setWorld(p2.p22.World world);

                public virtual extern p2.p22.Body[] getCollisionPairs(p2.p22.World world);

                public virtual extern bool boundingVolumeCheck(p2.p22.Body bodyA, p2.p22.Body bodyB);
            }

            public class GridBroadphase : p2.p22.Broadphase
            {
                private double xmink__BackingField;
                private double xmaxk__BackingField;
                private double ymink__BackingField;
                private double ymaxk__BackingField;
                private double nxk__BackingField;
                private double nyk__BackingField;
                private double binsizeXk__BackingField;
                private double binsizeYk__BackingField;
                private static double AABBk__BackingField;
                private static double BOUNDING_CIRCLEk__BackingField;
                private static double NAIVEk__BackingField;
                private static double SAPk__BackingField;

                public extern GridBroadphase();

                public extern GridBroadphase(p2.p22.GridBroadphase.Config options);

                public virtual double xmin
                {
                    get
                    {
                        return this.xmink__BackingField;
                    }
                    set
                    {
                        this.xmink__BackingField = value;
                    }
                }

                public virtual double xmax
                {
                    get
                    {
                        return this.xmaxk__BackingField;
                    }
                    set
                    {
                        this.xmaxk__BackingField = value;
                    }
                }

                public virtual double ymin
                {
                    get
                    {
                        return this.ymink__BackingField;
                    }
                    set
                    {
                        this.ymink__BackingField = value;
                    }
                }

                public virtual double ymax
                {
                    get
                    {
                        return this.ymaxk__BackingField;
                    }
                    set
                    {
                        this.ymaxk__BackingField = value;
                    }
                }

                public virtual double nx
                {
                    get
                    {
                        return this.nxk__BackingField;
                    }
                    set
                    {
                        this.nxk__BackingField = value;
                    }
                }

                public virtual double ny
                {
                    get
                    {
                        return this.nyk__BackingField;
                    }
                    set
                    {
                        this.nyk__BackingField = value;
                    }
                }

                public virtual double binsizeX
                {
                    get
                    {
                        return this.binsizeXk__BackingField;
                    }
                    set
                    {
                        this.binsizeXk__BackingField = value;
                    }
                }

                public virtual double binsizeY
                {
                    get
                    {
                        return this.binsizeYk__BackingField;
                    }
                    set
                    {
                        this.binsizeYk__BackingField = value;
                    }
                }

                public new static double AABB
                {
                    get
                    {
                        return p2.p22.GridBroadphase.AABBk__BackingField;
                    }
                    set
                    {
                        p2.p22.GridBroadphase.AABBk__BackingField = value;
                    }
                }

                public new static double BOUNDING_CIRCLE
                {
                    get
                    {
                        return p2.p22.GridBroadphase.BOUNDING_CIRCLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.GridBroadphase.BOUNDING_CIRCLEk__BackingField = value;
                    }
                }

                public new static double NAIVE
                {
                    get
                    {
                        return p2.p22.GridBroadphase.NAIVEk__BackingField;
                    }
                    set
                    {
                        p2.p22.GridBroadphase.NAIVEk__BackingField = value;
                    }
                }

                public new static double SAP
                {
                    get
                    {
                        return p2.p22.GridBroadphase.SAPk__BackingField;
                    }
                    set
                    {
                        p2.p22.GridBroadphase.SAPk__BackingField = value;
                    }
                }

                public new static extern bool boundingRadiusCheck(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public new static extern bool aabbCheck(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public new static extern bool canCollide(p2.p22.Body bodyA, p2.p22.Body bodyB);

                [ObjectLiteral]
                public class Config : IObject
                {
                    private double? xmink__BackingField;
                    private double? xmaxk__BackingField;
                    private double? ymink__BackingField;
                    private double? ymaxk__BackingField;
                    private double? nxk__BackingField;
                    private double? nyk__BackingField;

                    public double? xmin
                    {
                        get
                        {
                            return this.xmink__BackingField;
                        }
                        set
                        {
                            this.xmink__BackingField = value;
                        }
                    }

                    public double? xmax
                    {
                        get
                        {
                            return this.xmaxk__BackingField;
                        }
                        set
                        {
                            this.xmaxk__BackingField = value;
                        }
                    }

                    public double? ymin
                    {
                        get
                        {
                            return this.ymink__BackingField;
                        }
                        set
                        {
                            this.ymink__BackingField = value;
                        }
                    }

                    public double? ymax
                    {
                        get
                        {
                            return this.ymaxk__BackingField;
                        }
                        set
                        {
                            this.ymaxk__BackingField = value;
                        }
                    }

                    public double? nx
                    {
                        get
                        {
                            return this.nxk__BackingField;
                        }
                        set
                        {
                            this.nxk__BackingField = value;
                        }
                    }

                    public double? ny
                    {
                        get
                        {
                            return this.nyk__BackingField;
                        }
                        set
                        {
                            this.nyk__BackingField = value;
                        }
                    }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class NativeBroadphase : p2.p22.Broadphase
            {
                private static double AABBk__BackingField;
                private static double BOUNDING_CIRCLEk__BackingField;
                private static double NAIVEk__BackingField;
                private static double SAPk__BackingField;

                public extern NativeBroadphase(double type);

                public new static double AABB
                {
                    get
                    {
                        return p2.p22.NativeBroadphase.AABBk__BackingField;
                    }
                    set
                    {
                        p2.p22.NativeBroadphase.AABBk__BackingField = value;
                    }
                }

                public new static double BOUNDING_CIRCLE
                {
                    get
                    {
                        return p2.p22.NativeBroadphase.BOUNDING_CIRCLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.NativeBroadphase.BOUNDING_CIRCLEk__BackingField = value;
                    }
                }

                public new static double NAIVE
                {
                    get
                    {
                        return p2.p22.NativeBroadphase.NAIVEk__BackingField;
                    }
                    set
                    {
                        p2.p22.NativeBroadphase.NAIVEk__BackingField = value;
                    }
                }

                public new static double SAP
                {
                    get
                    {
                        return p2.p22.NativeBroadphase.SAPk__BackingField;
                    }
                    set
                    {
                        p2.p22.NativeBroadphase.SAPk__BackingField = value;
                    }
                }

                public new static extern bool boundingRadiusCheck(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public new static extern bool aabbCheck(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public new static extern bool canCollide(p2.p22.Body bodyA, p2.p22.Body bodyB);
            }

            public class Narrowphase : IObject
            {
                private p2.p22.ContactEquation[] contactEquationsk__BackingField;
                private p2.p22.FrictionEquation[] frictionEquationsk__BackingField;
                private bool enableFrictionk__BackingField;
                private bool enableEquationsk__BackingField;
                private double slipForcek__BackingField;
                private double frictionCoefficientk__BackingField;
                private double surfaceVelocityk__BackingField;
                private bool reuseObjectsk__BackingField;
                private object[] resuableContactEquationsk__BackingField;
                private object[] reusableFrictionEquationsk__BackingField;
                private double restitutionk__BackingField;
                private double stiffnessk__BackingField;
                private double relaxationk__BackingField;
                private double frictionStiffnessk__BackingField;
                private double frictionRelaxationk__BackingField;
                private bool enableFrictionReductionk__BackingField;
                private double contactSkinSizek__BackingField;

                public virtual p2.p22.ContactEquation[] contactEquations
                {
                    get
                    {
                        return this.contactEquationsk__BackingField;
                    }
                    set
                    {
                        this.contactEquationsk__BackingField = value;
                    }
                }

                public virtual p2.p22.FrictionEquation[] frictionEquations
                {
                    get
                    {
                        return this.frictionEquationsk__BackingField;
                    }
                    set
                    {
                        this.frictionEquationsk__BackingField = value;
                    }
                }

                public virtual bool enableFriction
                {
                    get
                    {
                        return this.enableFrictionk__BackingField;
                    }
                    set
                    {
                        this.enableFrictionk__BackingField = value;
                    }
                }

                public virtual bool enableEquations
                {
                    get
                    {
                        return this.enableEquationsk__BackingField;
                    }
                    set
                    {
                        this.enableEquationsk__BackingField = value;
                    }
                }

                public virtual double slipForce
                {
                    get
                    {
                        return this.slipForcek__BackingField;
                    }
                    set
                    {
                        this.slipForcek__BackingField = value;
                    }
                }

                public virtual double frictionCoefficient
                {
                    get
                    {
                        return this.frictionCoefficientk__BackingField;
                    }
                    set
                    {
                        this.frictionCoefficientk__BackingField = value;
                    }
                }

                public virtual double surfaceVelocity
                {
                    get
                    {
                        return this.surfaceVelocityk__BackingField;
                    }
                    set
                    {
                        this.surfaceVelocityk__BackingField = value;
                    }
                }

                public virtual bool reuseObjects
                {
                    get
                    {
                        return this.reuseObjectsk__BackingField;
                    }
                    set
                    {
                        this.reuseObjectsk__BackingField = value;
                    }
                }

                public virtual object[] resuableContactEquations
                {
                    get
                    {
                        return this.resuableContactEquationsk__BackingField;
                    }
                    set
                    {
                        this.resuableContactEquationsk__BackingField = value;
                    }
                }

                public virtual object[] reusableFrictionEquations
                {
                    get
                    {
                        return this.reusableFrictionEquationsk__BackingField;
                    }
                    set
                    {
                        this.reusableFrictionEquationsk__BackingField = value;
                    }
                }

                public virtual double restitution
                {
                    get
                    {
                        return this.restitutionk__BackingField;
                    }
                    set
                    {
                        this.restitutionk__BackingField = value;
                    }
                }

                public virtual double stiffness
                {
                    get
                    {
                        return this.stiffnessk__BackingField;
                    }
                    set
                    {
                        this.stiffnessk__BackingField = value;
                    }
                }

                public virtual double relaxation
                {
                    get
                    {
                        return this.relaxationk__BackingField;
                    }
                    set
                    {
                        this.relaxationk__BackingField = value;
                    }
                }

                public virtual double frictionStiffness
                {
                    get
                    {
                        return this.frictionStiffnessk__BackingField;
                    }
                    set
                    {
                        this.frictionStiffnessk__BackingField = value;
                    }
                }

                public virtual double frictionRelaxation
                {
                    get
                    {
                        return this.frictionRelaxationk__BackingField;
                    }
                    set
                    {
                        this.frictionRelaxationk__BackingField = value;
                    }
                }

                public virtual bool enableFrictionReduction
                {
                    get
                    {
                        return this.enableFrictionReductionk__BackingField;
                    }
                    set
                    {
                        this.enableFrictionReductionk__BackingField = value;
                    }
                }

                public virtual double contactSkinSize
                {
                    get
                    {
                        return this.contactSkinSizek__BackingField;
                    }
                    set
                    {
                        this.contactSkinSizek__BackingField = value;
                    }
                }

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
                private p2.p22.Body[] axisListk__BackingField;
                private double axisIndexk__BackingField;
                private static double AABBk__BackingField;
                private static double BOUNDING_CIRCLEk__BackingField;
                private static double NAIVEk__BackingField;
                private static double SAPk__BackingField;

                public extern SAPBroadphase(double type);

                public virtual p2.p22.Body[] axisList
                {
                    get
                    {
                        return this.axisListk__BackingField;
                    }
                    set
                    {
                        this.axisListk__BackingField = value;
                    }
                }

                public virtual double axisIndex
                {
                    get
                    {
                        return this.axisIndexk__BackingField;
                    }
                    set
                    {
                        this.axisIndexk__BackingField = value;
                    }
                }

                public new static double AABB
                {
                    get
                    {
                        return p2.p22.SAPBroadphase.AABBk__BackingField;
                    }
                    set
                    {
                        p2.p22.SAPBroadphase.AABBk__BackingField = value;
                    }
                }

                public new static double BOUNDING_CIRCLE
                {
                    get
                    {
                        return p2.p22.SAPBroadphase.BOUNDING_CIRCLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.SAPBroadphase.BOUNDING_CIRCLEk__BackingField = value;
                    }
                }

                public new static double NAIVE
                {
                    get
                    {
                        return p2.p22.SAPBroadphase.NAIVEk__BackingField;
                    }
                    set
                    {
                        p2.p22.SAPBroadphase.NAIVEk__BackingField = value;
                    }
                }

                public new static double SAP
                {
                    get
                    {
                        return p2.p22.SAPBroadphase.SAPk__BackingField;
                    }
                    set
                    {
                        p2.p22.SAPBroadphase.SAPk__BackingField = value;
                    }
                }

                public new static extern bool boundingRadiusCheck(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public new static extern bool aabbCheck(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public new static extern bool canCollide(p2.p22.Body bodyA, p2.p22.Body bodyB);
            }

            public class Constraint : IObject
            {
                private static double DISTANCEk__BackingField;
                private static double GEARk__BackingField;
                private static double LOCKk__BackingField;
                private static double PRISMATICk__BackingField;
                private static double REVOLUTEk__BackingField;
                private double typek__BackingField;
                private p2.p22.Equation[] equeationsk__BackingField;
                private p2.p22.Body bodyAk__BackingField;
                private p2.p22.Body bodyBk__BackingField;
                private bool collideConnectedk__BackingField;

                public static double DISTANCE
                {
                    get
                    {
                        return p2.p22.Constraint.DISTANCEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Constraint.DISTANCEk__BackingField = value;
                    }
                }

                public static double GEAR
                {
                    get
                    {
                        return p2.p22.Constraint.GEARk__BackingField;
                    }
                    set
                    {
                        p2.p22.Constraint.GEARk__BackingField = value;
                    }
                }

                public static double LOCK
                {
                    get
                    {
                        return p2.p22.Constraint.LOCKk__BackingField;
                    }
                    set
                    {
                        p2.p22.Constraint.LOCKk__BackingField = value;
                    }
                }

                public static double PRISMATIC
                {
                    get
                    {
                        return p2.p22.Constraint.PRISMATICk__BackingField;
                    }
                    set
                    {
                        p2.p22.Constraint.PRISMATICk__BackingField = value;
                    }
                }

                public static double REVOLUTE
                {
                    get
                    {
                        return p2.p22.Constraint.REVOLUTEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Constraint.REVOLUTEk__BackingField = value;
                    }
                }

                public extern Constraint(p2.p22.Body bodyA, p2.p22.Body bodyB, double type);

                public extern Constraint(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  double type,
                  p2.p22.Constraint.Config options);

                public virtual double type
                {
                    get
                    {
                        return this.typek__BackingField;
                    }
                    set
                    {
                        this.typek__BackingField = value;
                    }
                }

                public virtual p2.p22.Equation[] equeations
                {
                    get
                    {
                        return this.equeationsk__BackingField;
                    }
                    set
                    {
                        this.equeationsk__BackingField = value;
                    }
                }

                public virtual p2.p22.Body bodyA
                {
                    get
                    {
                        return this.bodyAk__BackingField;
                    }
                    set
                    {
                        this.bodyAk__BackingField = value;
                    }
                }

                public virtual p2.p22.Body bodyB
                {
                    get
                    {
                        return this.bodyBk__BackingField;
                    }
                    set
                    {
                        this.bodyBk__BackingField = value;
                    }
                }

                public virtual bool collideConnected
                {
                    get
                    {
                        return this.collideConnectedk__BackingField;
                    }
                    set
                    {
                        this.collideConnectedk__BackingField = value;
                    }
                }

                public virtual extern void update();

                public virtual extern void setStiffness(double stiffness);

                public virtual extern void setRelaxation(double relaxation);

                [ObjectLiteral]
                public class Config : IObject
                {
                    private bool? collideConnectedk__BackingField;

                    public bool? collideConnected
                    {
                        get
                        {
                            return this.collideConnectedk__BackingField;
                        }
                        set
                        {
                            this.collideConnectedk__BackingField = value;
                        }
                    }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class DistanceConstraint : p2.p22.Constraint
            {
                private double[] localAnchorAk__BackingField;
                private double[] localAnchorBk__BackingField;
                private double distancek__BackingField;
                private double maxForcek__BackingField;
                private bool upperLimitEnabledk__BackingField;
                private double upperLimitk__BackingField;
                private bool lowerLimitEnabledk__BackingField;
                private double lowerLimitk__BackingField;
                private double positionk__BackingField;
                private static double DISTANCEk__BackingField;
                private static double GEARk__BackingField;
                private static double LOCKk__BackingField;
                private static double PRISMATICk__BackingField;
                private static double REVOLUTEk__BackingField;

                public extern DistanceConstraint(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern DistanceConstraint(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  p2.p22.DistanceConstraint.Config options);

                public virtual double[] localAnchorA
                {
                    get
                    {
                        return this.localAnchorAk__BackingField;
                    }
                    set
                    {
                        this.localAnchorAk__BackingField = value;
                    }
                }

                public virtual double[] localAnchorB
                {
                    get
                    {
                        return this.localAnchorBk__BackingField;
                    }
                    set
                    {
                        this.localAnchorBk__BackingField = value;
                    }
                }

                public virtual double distance
                {
                    get
                    {
                        return this.distancek__BackingField;
                    }
                    set
                    {
                        this.distancek__BackingField = value;
                    }
                }

                public virtual double maxForce
                {
                    get
                    {
                        return this.maxForcek__BackingField;
                    }
                    set
                    {
                        this.maxForcek__BackingField = value;
                    }
                }

                public virtual bool upperLimitEnabled
                {
                    get
                    {
                        return this.upperLimitEnabledk__BackingField;
                    }
                    set
                    {
                        this.upperLimitEnabledk__BackingField = value;
                    }
                }

                public virtual double upperLimit
                {
                    get
                    {
                        return this.upperLimitk__BackingField;
                    }
                    set
                    {
                        this.upperLimitk__BackingField = value;
                    }
                }

                public virtual bool lowerLimitEnabled
                {
                    get
                    {
                        return this.lowerLimitEnabledk__BackingField;
                    }
                    set
                    {
                        this.lowerLimitEnabledk__BackingField = value;
                    }
                }

                public virtual double lowerLimit
                {
                    get
                    {
                        return this.lowerLimitk__BackingField;
                    }
                    set
                    {
                        this.lowerLimitk__BackingField = value;
                    }
                }

                public virtual double position
                {
                    get
                    {
                        return this.positionk__BackingField;
                    }
                    set
                    {
                        this.positionk__BackingField = value;
                    }
                }

                public virtual extern void setMaxForce(double f);

                public virtual extern double getMaxForce();

                public new static double DISTANCE
                {
                    get
                    {
                        return p2.p22.DistanceConstraint.DISTANCEk__BackingField;
                    }
                    set
                    {
                        p2.p22.DistanceConstraint.DISTANCEk__BackingField = value;
                    }
                }

                public new static double GEAR
                {
                    get
                    {
                        return p2.p22.DistanceConstraint.GEARk__BackingField;
                    }
                    set
                    {
                        p2.p22.DistanceConstraint.GEARk__BackingField = value;
                    }
                }

                public new static double LOCK
                {
                    get
                    {
                        return p2.p22.DistanceConstraint.LOCKk__BackingField;
                    }
                    set
                    {
                        p2.p22.DistanceConstraint.LOCKk__BackingField = value;
                    }
                }

                public new static double PRISMATIC
                {
                    get
                    {
                        return p2.p22.DistanceConstraint.PRISMATICk__BackingField;
                    }
                    set
                    {
                        p2.p22.DistanceConstraint.PRISMATICk__BackingField = value;
                    }
                }

                public new static double REVOLUTE
                {
                    get
                    {
                        return p2.p22.DistanceConstraint.REVOLUTEk__BackingField;
                    }
                    set
                    {
                        p2.p22.DistanceConstraint.REVOLUTEk__BackingField = value;
                    }
                }

                [ObjectLiteral]
                public new class Config : IObject
                {
                    private double? distancek__BackingField;
                    private double[] localAnchorAk__BackingField;
                    private double[] localAnchorBk__BackingField;
                    private double? maxForcek__BackingField;

                    public double? distance
                    {
                        get
                        {
                            return this.distancek__BackingField;
                        }
                        set
                        {
                            this.distancek__BackingField = value;
                        }
                    }

                    public double[] localAnchorA
                    {
                        get
                        {
                            return this.localAnchorAk__BackingField;
                        }
                        set
                        {
                            this.localAnchorAk__BackingField = value;
                        }
                    }

                    public double[] localAnchorB
                    {
                        get
                        {
                            return this.localAnchorBk__BackingField;
                        }
                        set
                        {
                            this.localAnchorBk__BackingField = value;
                        }
                    }

                    public double? maxForce
                    {
                        get
                        {
                            return this.maxForcek__BackingField;
                        }
                        set
                        {
                            this.maxForcek__BackingField = value;
                        }
                    }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class GearConstraint : p2.p22.Constraint
            {
                private double ratiok__BackingField;
                private double anglek__BackingField;
                private static double DISTANCEk__BackingField;
                private static double GEARk__BackingField;
                private static double LOCKk__BackingField;
                private static double PRISMATICk__BackingField;
                private static double REVOLUTEk__BackingField;

                public extern GearConstraint(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern GearConstraint(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  p2.p22.GearConstraint.Config options);

                public virtual double ratio
                {
                    get
                    {
                        return this.ratiok__BackingField;
                    }
                    set
                    {
                        this.ratiok__BackingField = value;
                    }
                }

                public virtual double angle
                {
                    get
                    {
                        return this.anglek__BackingField;
                    }
                    set
                    {
                        this.anglek__BackingField = value;
                    }
                }

                public virtual extern void setMaxTorque(double torque);

                public virtual extern double getMaxTorque();

                public new static double DISTANCE
                {
                    get
                    {
                        return p2.p22.GearConstraint.DISTANCEk__BackingField;
                    }
                    set
                    {
                        p2.p22.GearConstraint.DISTANCEk__BackingField = value;
                    }
                }

                public new static double GEAR
                {
                    get
                    {
                        return p2.p22.GearConstraint.GEARk__BackingField;
                    }
                    set
                    {
                        p2.p22.GearConstraint.GEARk__BackingField = value;
                    }
                }

                public new static double LOCK
                {
                    get
                    {
                        return p2.p22.GearConstraint.LOCKk__BackingField;
                    }
                    set
                    {
                        p2.p22.GearConstraint.LOCKk__BackingField = value;
                    }
                }

                public new static double PRISMATIC
                {
                    get
                    {
                        return p2.p22.GearConstraint.PRISMATICk__BackingField;
                    }
                    set
                    {
                        p2.p22.GearConstraint.PRISMATICk__BackingField = value;
                    }
                }

                public new static double REVOLUTE
                {
                    get
                    {
                        return p2.p22.GearConstraint.REVOLUTEk__BackingField;
                    }
                    set
                    {
                        p2.p22.GearConstraint.REVOLUTEk__BackingField = value;
                    }
                }

                [ObjectLiteral]
                public new class Config : IObject
                {
                    private double? anglek__BackingField;
                    private double? ratiok__BackingField;
                    private double? maxTorquek__BackingField;

                    public double? angle
                    {
                        get
                        {
                            return this.anglek__BackingField;
                        }
                        set
                        {
                            this.anglek__BackingField = value;
                        }
                    }

                    public double? ratio
                    {
                        get
                        {
                            return this.ratiok__BackingField;
                        }
                        set
                        {
                            this.ratiok__BackingField = value;
                        }
                    }

                    public double? maxTorque
                    {
                        get
                        {
                            return this.maxTorquek__BackingField;
                        }
                        set
                        {
                            this.maxTorquek__BackingField = value;
                        }
                    }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class LockConstraint : p2.p22.Constraint
            {
                private static double DISTANCEk__BackingField;
                private static double GEARk__BackingField;
                private static double LOCKk__BackingField;
                private static double PRISMATICk__BackingField;
                private static double REVOLUTEk__BackingField;

                public extern LockConstraint(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern LockConstraint(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  p2.p22.LockConstraint.Config options);

                public virtual extern void setMaxForce(double force);

                public virtual extern double getMaxForce();

                public new static double DISTANCE
                {
                    get
                    {
                        return p2.p22.LockConstraint.DISTANCEk__BackingField;
                    }
                    set
                    {
                        p2.p22.LockConstraint.DISTANCEk__BackingField = value;
                    }
                }

                public new static double GEAR
                {
                    get
                    {
                        return p2.p22.LockConstraint.GEARk__BackingField;
                    }
                    set
                    {
                        p2.p22.LockConstraint.GEARk__BackingField = value;
                    }
                }

                public new static double LOCK
                {
                    get
                    {
                        return p2.p22.LockConstraint.LOCKk__BackingField;
                    }
                    set
                    {
                        p2.p22.LockConstraint.LOCKk__BackingField = value;
                    }
                }

                public new static double PRISMATIC
                {
                    get
                    {
                        return p2.p22.LockConstraint.PRISMATICk__BackingField;
                    }
                    set
                    {
                        p2.p22.LockConstraint.PRISMATICk__BackingField = value;
                    }
                }

                public new static double REVOLUTE
                {
                    get
                    {
                        return p2.p22.LockConstraint.REVOLUTEk__BackingField;
                    }
                    set
                    {
                        p2.p22.LockConstraint.REVOLUTEk__BackingField = value;
                    }
                }

                [ObjectLiteral]
                public new class Config : IObject
                {
                    private double[] localOffsetBk__BackingField;
                    private double? localAngleBk__BackingField;
                    private double? maxForcek__BackingField;

                    public double[] localOffsetB
                    {
                        get
                        {
                            return this.localOffsetBk__BackingField;
                        }
                        set
                        {
                            this.localOffsetBk__BackingField = value;
                        }
                    }

                    public double? localAngleB
                    {
                        get
                        {
                            return this.localAngleBk__BackingField;
                        }
                        set
                        {
                            this.localAngleBk__BackingField = value;
                        }
                    }

                    public double? maxForce
                    {
                        get
                        {
                            return this.maxForcek__BackingField;
                        }
                        set
                        {
                            this.maxForcek__BackingField = value;
                        }
                    }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class PrismaticConstraint : p2.p22.Constraint
            {
                private double[] localAnchorAk__BackingField;
                private double[] localAnchorBk__BackingField;
                private double[] localAxisAk__BackingField;
                private double positionk__BackingField;
                private double velocityk__BackingField;
                private bool lowerLimitEnabledk__BackingField;
                private bool upperLimitEnabledk__BackingField;
                private double lowerLimitk__BackingField;
                private double upperLimitk__BackingField;
                private p2.p22.ContactEquation upperLimitEquationk__BackingField;
                private p2.p22.ContactEquation lowerLimitEquationk__BackingField;
                private p2.p22.Equation motorEquationk__BackingField;
                private bool motorEnabledk__BackingField;
                private double motorSpeedk__BackingField;
                private static double DISTANCEk__BackingField;
                private static double GEARk__BackingField;
                private static double LOCKk__BackingField;
                private static double PRISMATICk__BackingField;
                private static double REVOLUTEk__BackingField;

                public extern PrismaticConstraint(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern PrismaticConstraint(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  p2.p22.PrismaticConstraint.Config options);

                public virtual double[] localAnchorA
                {
                    get
                    {
                        return this.localAnchorAk__BackingField;
                    }
                    set
                    {
                        this.localAnchorAk__BackingField = value;
                    }
                }

                public virtual double[] localAnchorB
                {
                    get
                    {
                        return this.localAnchorBk__BackingField;
                    }
                    set
                    {
                        this.localAnchorBk__BackingField = value;
                    }
                }

                public virtual double[] localAxisA
                {
                    get
                    {
                        return this.localAxisAk__BackingField;
                    }
                    set
                    {
                        this.localAxisAk__BackingField = value;
                    }
                }

                public virtual double position
                {
                    get
                    {
                        return this.positionk__BackingField;
                    }
                    set
                    {
                        this.positionk__BackingField = value;
                    }
                }

                public virtual double velocity
                {
                    get
                    {
                        return this.velocityk__BackingField;
                    }
                    set
                    {
                        this.velocityk__BackingField = value;
                    }
                }

                public virtual bool lowerLimitEnabled
                {
                    get
                    {
                        return this.lowerLimitEnabledk__BackingField;
                    }
                    set
                    {
                        this.lowerLimitEnabledk__BackingField = value;
                    }
                }

                public virtual bool upperLimitEnabled
                {
                    get
                    {
                        return this.upperLimitEnabledk__BackingField;
                    }
                    set
                    {
                        this.upperLimitEnabledk__BackingField = value;
                    }
                }

                public virtual double lowerLimit
                {
                    get
                    {
                        return this.lowerLimitk__BackingField;
                    }
                    set
                    {
                        this.lowerLimitk__BackingField = value;
                    }
                }

                public virtual double upperLimit
                {
                    get
                    {
                        return this.upperLimitk__BackingField;
                    }
                    set
                    {
                        this.upperLimitk__BackingField = value;
                    }
                }

                public virtual p2.p22.ContactEquation upperLimitEquation
                {
                    get
                    {
                        return this.upperLimitEquationk__BackingField;
                    }
                    set
                    {
                        this.upperLimitEquationk__BackingField = value;
                    }
                }

                public virtual p2.p22.ContactEquation lowerLimitEquation
                {
                    get
                    {
                        return this.lowerLimitEquationk__BackingField;
                    }
                    set
                    {
                        this.lowerLimitEquationk__BackingField = value;
                    }
                }

                public virtual p2.p22.Equation motorEquation
                {
                    get
                    {
                        return this.motorEquationk__BackingField;
                    }
                    set
                    {
                        this.motorEquationk__BackingField = value;
                    }
                }

                public virtual bool motorEnabled
                {
                    get
                    {
                        return this.motorEnabledk__BackingField;
                    }
                    set
                    {
                        this.motorEnabledk__BackingField = value;
                    }
                }

                public virtual double motorSpeed
                {
                    get
                    {
                        return this.motorSpeedk__BackingField;
                    }
                    set
                    {
                        this.motorSpeedk__BackingField = value;
                    }
                }

                public virtual extern void enableMotor();

                public virtual extern void disableMotor();

                public virtual extern void setLimits(double lower, double upper);

                public new static double DISTANCE
                {
                    get
                    {
                        return p2.p22.PrismaticConstraint.DISTANCEk__BackingField;
                    }
                    set
                    {
                        p2.p22.PrismaticConstraint.DISTANCEk__BackingField = value;
                    }
                }

                public new static double GEAR
                {
                    get
                    {
                        return p2.p22.PrismaticConstraint.GEARk__BackingField;
                    }
                    set
                    {
                        p2.p22.PrismaticConstraint.GEARk__BackingField = value;
                    }
                }

                public new static double LOCK
                {
                    get
                    {
                        return p2.p22.PrismaticConstraint.LOCKk__BackingField;
                    }
                    set
                    {
                        p2.p22.PrismaticConstraint.LOCKk__BackingField = value;
                    }
                }

                public new static double PRISMATIC
                {
                    get
                    {
                        return p2.p22.PrismaticConstraint.PRISMATICk__BackingField;
                    }
                    set
                    {
                        p2.p22.PrismaticConstraint.PRISMATICk__BackingField = value;
                    }
                }

                public new static double REVOLUTE
                {
                    get
                    {
                        return p2.p22.PrismaticConstraint.REVOLUTEk__BackingField;
                    }
                    set
                    {
                        p2.p22.PrismaticConstraint.REVOLUTEk__BackingField = value;
                    }
                }

                [ObjectLiteral]
                public new class Config : IObject
                {
                    private double? maxForcek__BackingField;
                    private double[] localAnchorAk__BackingField;
                    private double[] localAnchorBk__BackingField;
                    private double[] localAxisAk__BackingField;
                    private bool? disableRotationalLockk__BackingField;
                    private double? upperLimitk__BackingField;
                    private double? lowerLimitk__BackingField;

                    public double? maxForce
                    {
                        get
                        {
                            return this.maxForcek__BackingField;
                        }
                        set
                        {
                            this.maxForcek__BackingField = value;
                        }
                    }

                    public double[] localAnchorA
                    {
                        get
                        {
                            return this.localAnchorAk__BackingField;
                        }
                        set
                        {
                            this.localAnchorAk__BackingField = value;
                        }
                    }

                    public double[] localAnchorB
                    {
                        get
                        {
                            return this.localAnchorBk__BackingField;
                        }
                        set
                        {
                            this.localAnchorBk__BackingField = value;
                        }
                    }

                    public double[] localAxisA
                    {
                        get
                        {
                            return this.localAxisAk__BackingField;
                        }
                        set
                        {
                            this.localAxisAk__BackingField = value;
                        }
                    }

                    public bool? disableRotationalLock
                    {
                        get
                        {
                            return this.disableRotationalLockk__BackingField;
                        }
                        set
                        {
                            this.disableRotationalLockk__BackingField = value;
                        }
                    }

                    public double? upperLimit
                    {
                        get
                        {
                            return this.upperLimitk__BackingField;
                        }
                        set
                        {
                            this.upperLimitk__BackingField = value;
                        }
                    }

                    public double? lowerLimit
                    {
                        get
                        {
                            return this.lowerLimitk__BackingField;
                        }
                        set
                        {
                            this.lowerLimitk__BackingField = value;
                        }
                    }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class RevoluteConstraint : p2.p22.Constraint
            {
                private double[] pivotAk__BackingField;
                private double[] pivotBk__BackingField;
                private p2.p22.RotationalVelocityEquation motorEquationk__BackingField;
                private bool motorEnabledk__BackingField;
                private double anglek__BackingField;
                private bool lowerLimitEnabledk__BackingField;
                private bool upperLimitEnabledk__BackingField;
                private double lowerLimitk__BackingField;
                private double upperLimitk__BackingField;
                private p2.p22.ContactEquation upperLimitEquationk__BackingField;
                private p2.p22.ContactEquation lowerLimitEquationk__BackingField;
                private static double DISTANCEk__BackingField;
                private static double GEARk__BackingField;
                private static double LOCKk__BackingField;
                private static double PRISMATICk__BackingField;
                private static double REVOLUTEk__BackingField;

                public extern RevoluteConstraint(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern RevoluteConstraint(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  p2.p22.RevoluteConstraint.Config options);

                public virtual double[] pivotA
                {
                    get
                    {
                        return this.pivotAk__BackingField;
                    }
                    set
                    {
                        this.pivotAk__BackingField = value;
                    }
                }

                public virtual double[] pivotB
                {
                    get
                    {
                        return this.pivotBk__BackingField;
                    }
                    set
                    {
                        this.pivotBk__BackingField = value;
                    }
                }

                public virtual p2.p22.RotationalVelocityEquation motorEquation
                {
                    get
                    {
                        return this.motorEquationk__BackingField;
                    }
                    set
                    {
                        this.motorEquationk__BackingField = value;
                    }
                }

                public virtual bool motorEnabled
                {
                    get
                    {
                        return this.motorEnabledk__BackingField;
                    }
                    set
                    {
                        this.motorEnabledk__BackingField = value;
                    }
                }

                public virtual double angle
                {
                    get
                    {
                        return this.anglek__BackingField;
                    }
                    set
                    {
                        this.anglek__BackingField = value;
                    }
                }

                public virtual bool lowerLimitEnabled
                {
                    get
                    {
                        return this.lowerLimitEnabledk__BackingField;
                    }
                    set
                    {
                        this.lowerLimitEnabledk__BackingField = value;
                    }
                }

                public virtual bool upperLimitEnabled
                {
                    get
                    {
                        return this.upperLimitEnabledk__BackingField;
                    }
                    set
                    {
                        this.upperLimitEnabledk__BackingField = value;
                    }
                }

                public virtual double lowerLimit
                {
                    get
                    {
                        return this.lowerLimitk__BackingField;
                    }
                    set
                    {
                        this.lowerLimitk__BackingField = value;
                    }
                }

                public virtual double upperLimit
                {
                    get
                    {
                        return this.upperLimitk__BackingField;
                    }
                    set
                    {
                        this.upperLimitk__BackingField = value;
                    }
                }

                public virtual p2.p22.ContactEquation upperLimitEquation
                {
                    get
                    {
                        return this.upperLimitEquationk__BackingField;
                    }
                    set
                    {
                        this.upperLimitEquationk__BackingField = value;
                    }
                }

                public virtual p2.p22.ContactEquation lowerLimitEquation
                {
                    get
                    {
                        return this.lowerLimitEquationk__BackingField;
                    }
                    set
                    {
                        this.lowerLimitEquationk__BackingField = value;
                    }
                }

                public virtual extern void enableMotor();

                public virtual extern void disableMotor();

                public virtual extern bool motorIsEnabled();

                public virtual extern void setLimits(double lower, double upper);

                public virtual extern void setMotorSpeed(double speed);

                public virtual extern double getMotorSpeed();

                public new static double DISTANCE
                {
                    get
                    {
                        return p2.p22.RevoluteConstraint.DISTANCEk__BackingField;
                    }
                    set
                    {
                        p2.p22.RevoluteConstraint.DISTANCEk__BackingField = value;
                    }
                }

                public new static double GEAR
                {
                    get
                    {
                        return p2.p22.RevoluteConstraint.GEARk__BackingField;
                    }
                    set
                    {
                        p2.p22.RevoluteConstraint.GEARk__BackingField = value;
                    }
                }

                public new static double LOCK
                {
                    get
                    {
                        return p2.p22.RevoluteConstraint.LOCKk__BackingField;
                    }
                    set
                    {
                        p2.p22.RevoluteConstraint.LOCKk__BackingField = value;
                    }
                }

                public new static double PRISMATIC
                {
                    get
                    {
                        return p2.p22.RevoluteConstraint.PRISMATICk__BackingField;
                    }
                    set
                    {
                        p2.p22.RevoluteConstraint.PRISMATICk__BackingField = value;
                    }
                }

                public new static double REVOLUTE
                {
                    get
                    {
                        return p2.p22.RevoluteConstraint.REVOLUTEk__BackingField;
                    }
                    set
                    {
                        p2.p22.RevoluteConstraint.REVOLUTEk__BackingField = value;
                    }
                }

                [ObjectLiteral]
                public new class Config : IObject
                {
                    private double[] worldPivotk__BackingField;
                    private double[] localPivotAk__BackingField;
                    private double[] localPivotBk__BackingField;
                    private double? maxForcek__BackingField;

                    public double[] worldPivot
                    {
                        get
                        {
                            return this.worldPivotk__BackingField;
                        }
                        set
                        {
                            this.worldPivotk__BackingField = value;
                        }
                    }

                    public double[] localPivotA
                    {
                        get
                        {
                            return this.localPivotAk__BackingField;
                        }
                        set
                        {
                            this.localPivotAk__BackingField = value;
                        }
                    }

                    public double[] localPivotB
                    {
                        get
                        {
                            return this.localPivotBk__BackingField;
                        }
                        set
                        {
                            this.localPivotBk__BackingField = value;
                        }
                    }

                    public double? maxForce
                    {
                        get
                        {
                            return this.maxForcek__BackingField;
                        }
                        set
                        {
                            this.maxForcek__BackingField = value;
                        }
                    }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class AngleLockEquation : p2.p22.Equation
            {
                private static double DEFAULT_STIFFNESSk__BackingField;
                private static double DEFAULT_RELAXATIONk__BackingField;

                public extern AngleLockEquation(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern AngleLockEquation(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  p2.p22.AngleLockEquation.Config options);

                public override extern double computeGq();

                public virtual extern double setRatio(double ratio);

                public virtual extern double setMaxTorque(double torque);

                public new static double DEFAULT_STIFFNESS
                {
                    get
                    {
                        return p2.p22.AngleLockEquation.DEFAULT_STIFFNESSk__BackingField;
                    }
                    set
                    {
                        p2.p22.AngleLockEquation.DEFAULT_STIFFNESSk__BackingField = value;
                    }
                }

                public new static double DEFAULT_RELAXATION
                {
                    get
                    {
                        return p2.p22.AngleLockEquation.DEFAULT_RELAXATIONk__BackingField;
                    }
                    set
                    {
                        p2.p22.AngleLockEquation.DEFAULT_RELAXATIONk__BackingField = value;
                    }
                }

                [ObjectLiteral]
                public class Config : IObject
                {
                    private double? anglek__BackingField;
                    private double? ratiok__BackingField;

                    public double? angle
                    {
                        get
                        {
                            return this.anglek__BackingField;
                        }
                        set
                        {
                            this.anglek__BackingField = value;
                        }
                    }

                    public double? ratio
                    {
                        get
                        {
                            return this.ratiok__BackingField;
                        }
                        set
                        {
                            this.ratiok__BackingField = value;
                        }
                    }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class ContactEquation : p2.p22.Equation
            {
                private double[] contactPointAk__BackingField;
                private double[] penetrationVeck__BackingField;
                private double[] contactPointBk__BackingField;
                private double[] normalAk__BackingField;
                private double restitutionk__BackingField;
                private bool firstImpactk__BackingField;
                private p2.p22.Shape shapeAk__BackingField;
                private p2.p22.Shape shapeBk__BackingField;
                private static double DEFAULT_STIFFNESSk__BackingField;
                private static double DEFAULT_RELAXATIONk__BackingField;

                public extern ContactEquation(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public virtual double[] contactPointA
                {
                    get
                    {
                        return this.contactPointAk__BackingField;
                    }
                    set
                    {
                        this.contactPointAk__BackingField = value;
                    }
                }

                public virtual double[] penetrationVec
                {
                    get
                    {
                        return this.penetrationVeck__BackingField;
                    }
                    set
                    {
                        this.penetrationVeck__BackingField = value;
                    }
                }

                public virtual double[] contactPointB
                {
                    get
                    {
                        return this.contactPointBk__BackingField;
                    }
                    set
                    {
                        this.contactPointBk__BackingField = value;
                    }
                }

                public virtual double[] normalA
                {
                    get
                    {
                        return this.normalAk__BackingField;
                    }
                    set
                    {
                        this.normalAk__BackingField = value;
                    }
                }

                public virtual double restitution
                {
                    get
                    {
                        return this.restitutionk__BackingField;
                    }
                    set
                    {
                        this.restitutionk__BackingField = value;
                    }
                }

                public virtual bool firstImpact
                {
                    get
                    {
                        return this.firstImpactk__BackingField;
                    }
                    set
                    {
                        this.firstImpactk__BackingField = value;
                    }
                }

                public virtual p2.p22.Shape shapeA
                {
                    get
                    {
                        return this.shapeAk__BackingField;
                    }
                    set
                    {
                        this.shapeAk__BackingField = value;
                    }
                }

                public virtual p2.p22.Shape shapeB
                {
                    get
                    {
                        return this.shapeBk__BackingField;
                    }
                    set
                    {
                        this.shapeBk__BackingField = value;
                    }
                }

                public override extern double computeB(double a, double b, double h);

                public new static double DEFAULT_STIFFNESS
                {
                    get
                    {
                        return p2.p22.ContactEquation.DEFAULT_STIFFNESSk__BackingField;
                    }
                    set
                    {
                        p2.p22.ContactEquation.DEFAULT_STIFFNESSk__BackingField = value;
                    }
                }

                public new static double DEFAULT_RELAXATION
                {
                    get
                    {
                        return p2.p22.ContactEquation.DEFAULT_RELAXATIONk__BackingField;
                    }
                    set
                    {
                        p2.p22.ContactEquation.DEFAULT_RELAXATIONk__BackingField = value;
                    }
                }
            }

            public class Equation : IObject
            {
                private static double DEFAULT_STIFFNESSk__BackingField;
                private static double DEFAULT_RELAXATIONk__BackingField;
                private double minForcek__BackingField;
                private double maxForcek__BackingField;
                private p2.p22.Body bodyAk__BackingField;
                private p2.p22.Body bodyBk__BackingField;
                private double stiffnessk__BackingField;
                private double relaxationk__BackingField;
                private double[] Gk__BackingField;
                private double offsetk__BackingField;
                private double ak__BackingField;
                private double bk__BackingField;
                private double epsilonk__BackingField;
                private double timeStepk__BackingField;
                private bool needsUpdatek__BackingField;
                private double multiplierk__BackingField;
                private double relativeVelocityk__BackingField;
                private bool enabledk__BackingField;

                public static double DEFAULT_STIFFNESS
                {
                    get
                    {
                        return p2.p22.Equation.DEFAULT_STIFFNESSk__BackingField;
                    }
                    set
                    {
                        p2.p22.Equation.DEFAULT_STIFFNESSk__BackingField = value;
                    }
                }

                public static double DEFAULT_RELAXATION
                {
                    get
                    {
                        return p2.p22.Equation.DEFAULT_RELAXATIONk__BackingField;
                    }
                    set
                    {
                        p2.p22.Equation.DEFAULT_RELAXATIONk__BackingField = value;
                    }
                }

                public extern Equation(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern Equation(p2.p22.Body bodyA, p2.p22.Body bodyB, double minForce);

                public extern Equation(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  double minForce,
                  double maxForce);

                public virtual double minForce
                {
                    get
                    {
                        return this.minForcek__BackingField;
                    }
                    set
                    {
                        this.minForcek__BackingField = value;
                    }
                }

                public virtual double maxForce
                {
                    get
                    {
                        return this.maxForcek__BackingField;
                    }
                    set
                    {
                        this.maxForcek__BackingField = value;
                    }
                }

                public virtual p2.p22.Body bodyA
                {
                    get
                    {
                        return this.bodyAk__BackingField;
                    }
                    set
                    {
                        this.bodyAk__BackingField = value;
                    }
                }

                public virtual p2.p22.Body bodyB
                {
                    get
                    {
                        return this.bodyBk__BackingField;
                    }
                    set
                    {
                        this.bodyBk__BackingField = value;
                    }
                }

                public virtual double stiffness
                {
                    get
                    {
                        return this.stiffnessk__BackingField;
                    }
                    set
                    {
                        this.stiffnessk__BackingField = value;
                    }
                }

                public virtual double relaxation
                {
                    get
                    {
                        return this.relaxationk__BackingField;
                    }
                    set
                    {
                        this.relaxationk__BackingField = value;
                    }
                }

                public virtual double[] G
                {
                    get
                    {
                        return this.Gk__BackingField;
                    }
                    set
                    {
                        this.Gk__BackingField = value;
                    }
                }

                public virtual double offset
                {
                    get
                    {
                        return this.offsetk__BackingField;
                    }
                    set
                    {
                        this.offsetk__BackingField = value;
                    }
                }

                public virtual double a
                {
                    get
                    {
                        return this.ak__BackingField;
                    }
                    set
                    {
                        this.ak__BackingField = value;
                    }
                }

                public virtual double b
                {
                    get
                    {
                        return this.bk__BackingField;
                    }
                    set
                    {
                        this.bk__BackingField = value;
                    }
                }

                public virtual double epsilon
                {
                    get
                    {
                        return this.epsilonk__BackingField;
                    }
                    set
                    {
                        this.epsilonk__BackingField = value;
                    }
                }

                public virtual double timeStep
                {
                    get
                    {
                        return this.timeStepk__BackingField;
                    }
                    set
                    {
                        this.timeStepk__BackingField = value;
                    }
                }

                public virtual bool needsUpdate
                {
                    get
                    {
                        return this.needsUpdatek__BackingField;
                    }
                    set
                    {
                        this.needsUpdatek__BackingField = value;
                    }
                }

                public virtual double multiplier
                {
                    get
                    {
                        return this.multiplierk__BackingField;
                    }
                    set
                    {
                        this.multiplierk__BackingField = value;
                    }
                }

                public virtual double relativeVelocity
                {
                    get
                    {
                        return this.relativeVelocityk__BackingField;
                    }
                    set
                    {
                        this.relativeVelocityk__BackingField = value;
                    }
                }

                public virtual bool enabled
                {
                    get
                    {
                        return this.enabledk__BackingField;
                    }
                    set
                    {
                        this.enabledk__BackingField = value;
                    }
                }

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
                private double[] contactPointAk__BackingField;
                private double[] contactPointBk__BackingField;
                private double[] tk__BackingField;
                private p2.p22.Shape shapeAk__BackingField;
                private p2.p22.Shape shapeBk__BackingField;
                private double frictionCoefficientk__BackingField;
                private static double DEFAULT_STIFFNESSk__BackingField;
                private static double DEFAULT_RELAXATIONk__BackingField;

                public extern FrictionEquation(p2.p22.Body bodyA, p2.p22.Body bodyB, double slipForce);

                public virtual double[] contactPointA
                {
                    get
                    {
                        return this.contactPointAk__BackingField;
                    }
                    set
                    {
                        this.contactPointAk__BackingField = value;
                    }
                }

                public virtual double[] contactPointB
                {
                    get
                    {
                        return this.contactPointBk__BackingField;
                    }
                    set
                    {
                        this.contactPointBk__BackingField = value;
                    }
                }

                public virtual double[] t
                {
                    get
                    {
                        return this.tk__BackingField;
                    }
                    set
                    {
                        this.tk__BackingField = value;
                    }
                }

                public virtual p2.p22.Shape shapeA
                {
                    get
                    {
                        return this.shapeAk__BackingField;
                    }
                    set
                    {
                        this.shapeAk__BackingField = value;
                    }
                }

                public virtual p2.p22.Shape shapeB
                {
                    get
                    {
                        return this.shapeBk__BackingField;
                    }
                    set
                    {
                        this.shapeBk__BackingField = value;
                    }
                }

                public virtual double frictionCoefficient
                {
                    get
                    {
                        return this.frictionCoefficientk__BackingField;
                    }
                    set
                    {
                        this.frictionCoefficientk__BackingField = value;
                    }
                }

                public virtual extern double setSlipForce(double slipForce);

                public virtual extern double getSlipForce();

                public override extern double computeB(double a, double b, double h);

                public new static double DEFAULT_STIFFNESS
                {
                    get
                    {
                        return p2.p22.FrictionEquation.DEFAULT_STIFFNESSk__BackingField;
                    }
                    set
                    {
                        p2.p22.FrictionEquation.DEFAULT_STIFFNESSk__BackingField = value;
                    }
                }

                public new static double DEFAULT_RELAXATION
                {
                    get
                    {
                        return p2.p22.FrictionEquation.DEFAULT_RELAXATIONk__BackingField;
                    }
                    set
                    {
                        p2.p22.FrictionEquation.DEFAULT_RELAXATIONk__BackingField = value;
                    }
                }
            }

            public class RotationalLockEquation : p2.p22.Equation
            {
                private double anglek__BackingField;
                private static double DEFAULT_STIFFNESSk__BackingField;
                private static double DEFAULT_RELAXATIONk__BackingField;

                public extern RotationalLockEquation(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern RotationalLockEquation(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  p2.p22.RotationalLockEquation.Config options);

                public virtual double angle
                {
                    get
                    {
                        return this.anglek__BackingField;
                    }
                    set
                    {
                        this.anglek__BackingField = value;
                    }
                }

                public override extern double computeGq();

                public new static double DEFAULT_STIFFNESS
                {
                    get
                    {
                        return p2.p22.RotationalLockEquation.DEFAULT_STIFFNESSk__BackingField;
                    }
                    set
                    {
                        p2.p22.RotationalLockEquation.DEFAULT_STIFFNESSk__BackingField = value;
                    }
                }

                public new static double DEFAULT_RELAXATION
                {
                    get
                    {
                        return p2.p22.RotationalLockEquation.DEFAULT_RELAXATIONk__BackingField;
                    }
                    set
                    {
                        p2.p22.RotationalLockEquation.DEFAULT_RELAXATIONk__BackingField = value;
                    }
                }

                [ObjectLiteral]
                public class Config : IObject
                {
                    private double? anglek__BackingField;

                    public double? angle
                    {
                        get
                        {
                            return this.anglek__BackingField;
                        }
                        set
                        {
                            this.anglek__BackingField = value;
                        }
                    }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class RotationalVelocityEquation : p2.p22.Equation
            {
                private static double DEFAULT_STIFFNESSk__BackingField;
                private static double DEFAULT_RELAXATIONk__BackingField;

                public extern RotationalVelocityEquation(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public override extern double computeB(double a, double b, double h);

                public new static double DEFAULT_STIFFNESS
                {
                    get
                    {
                        return p2.p22.RotationalVelocityEquation.DEFAULT_STIFFNESSk__BackingField;
                    }
                    set
                    {
                        p2.p22.RotationalVelocityEquation.DEFAULT_STIFFNESSk__BackingField = value;
                    }
                }

                public new static double DEFAULT_RELAXATION
                {
                    get
                    {
                        return p2.p22.RotationalVelocityEquation.DEFAULT_RELAXATIONk__BackingField;
                    }
                    set
                    {
                        p2.p22.RotationalVelocityEquation.DEFAULT_RELAXATIONk__BackingField = value;
                    }
                }
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
                private double? frictionk__BackingField;
                private double? restitutionk__BackingField;
                private double? stiffnessk__BackingField;
                private double? relaxationk__BackingField;
                private double? frictionStiffnessk__BackingField;
                private double? frictionRelaxationk__BackingField;
                private double? surfaceVelocityk__BackingField;

                public virtual double? friction
                {
                    get
                    {
                        return this.frictionk__BackingField;
                    }
                    set
                    {
                        this.frictionk__BackingField = value;
                    }
                }

                public virtual double? restitution
                {
                    get
                    {
                        return this.restitutionk__BackingField;
                    }
                    set
                    {
                        this.restitutionk__BackingField = value;
                    }
                }

                public virtual double? stiffness
                {
                    get
                    {
                        return this.stiffnessk__BackingField;
                    }
                    set
                    {
                        this.stiffnessk__BackingField = value;
                    }
                }

                public virtual double? relaxation
                {
                    get
                    {
                        return this.relaxationk__BackingField;
                    }
                    set
                    {
                        this.relaxationk__BackingField = value;
                    }
                }

                public virtual double? frictionStiffness
                {
                    get
                    {
                        return this.frictionStiffnessk__BackingField;
                    }
                    set
                    {
                        this.frictionStiffnessk__BackingField = value;
                    }
                }

                public virtual double? frictionRelaxation
                {
                    get
                    {
                        return this.frictionRelaxationk__BackingField;
                    }
                    set
                    {
                        this.frictionRelaxationk__BackingField = value;
                    }
                }

                public virtual double? surfaceVelocity
                {
                    get
                    {
                        return this.surfaceVelocityk__BackingField;
                    }
                    set
                    {
                        this.surfaceVelocityk__BackingField = value;
                    }
                }

                public ContactMaterialOptions() : base()
                {
                    
                }
            }

            public class ContactMaterial : IObject
            {
                private static double idCounterk__BackingField;
                private double idk__BackingField;
                private p2.p22.Material materialAk__BackingField;
                private p2.p22.Material materialBk__BackingField;
                private double frictionk__BackingField;
                private double restitutionk__BackingField;
                private double stiffnessk__BackingField;
                private double relaxationk__BackingField;
                private double frictionStiffnessk__BackingField;
                private double frictionRelaxationk__BackingField;
                private double surfaceVelocityk__BackingField;
                private double contactSkinSizek__BackingField;

                public static double idCounter
                {
                    get
                    {
                        return p2.p22.ContactMaterial.idCounterk__BackingField;
                    }
                    set
                    {
                        p2.p22.ContactMaterial.idCounterk__BackingField = value;
                    }
                }

                public extern ContactMaterial(p2.p22.Material materialA, p2.p22.Material materialB);

                public extern ContactMaterial(
                  p2.p22.Material materialA,
                  p2.p22.Material materialB,
                  p2.p22.ContactMaterialOptions options);

                public virtual double id
                {
                    get
                    {
                        return this.idk__BackingField;
                    }
                    set
                    {
                        this.idk__BackingField = value;
                    }
                }

                public virtual p2.p22.Material materialA
                {
                    get
                    {
                        return this.materialAk__BackingField;
                    }
                    set
                    {
                        this.materialAk__BackingField = value;
                    }
                }

                public virtual p2.p22.Material materialB
                {
                    get
                    {
                        return this.materialBk__BackingField;
                    }
                    set
                    {
                        this.materialBk__BackingField = value;
                    }
                }

                public virtual double friction
                {
                    get
                    {
                        return this.frictionk__BackingField;
                    }
                    set
                    {
                        this.frictionk__BackingField = value;
                    }
                }

                public virtual double restitution
                {
                    get
                    {
                        return this.restitutionk__BackingField;
                    }
                    set
                    {
                        this.restitutionk__BackingField = value;
                    }
                }

                public virtual double stiffness
                {
                    get
                    {
                        return this.stiffnessk__BackingField;
                    }
                    set
                    {
                        this.stiffnessk__BackingField = value;
                    }
                }

                public virtual double relaxation
                {
                    get
                    {
                        return this.relaxationk__BackingField;
                    }
                    set
                    {
                        this.relaxationk__BackingField = value;
                    }
                }

                public virtual double frictionStiffness
                {
                    get
                    {
                        return this.frictionStiffnessk__BackingField;
                    }
                    set
                    {
                        this.frictionStiffnessk__BackingField = value;
                    }
                }

                public virtual double frictionRelaxation
                {
                    get
                    {
                        return this.frictionRelaxationk__BackingField;
                    }
                    set
                    {
                        this.frictionRelaxationk__BackingField = value;
                    }
                }

                public virtual double surfaceVelocity
                {
                    get
                    {
                        return this.surfaceVelocityk__BackingField;
                    }
                    set
                    {
                        this.surfaceVelocityk__BackingField = value;
                    }
                }

                public virtual double contactSkinSize
                {
                    get
                    {
                        return this.contactSkinSizek__BackingField;
                    }
                    set
                    {
                        this.contactSkinSizek__BackingField = value;
                    }
                }
            }

            public class Material : IObject
            {
                private static double idCounterk__BackingField;
                private double idk__BackingField;

                public static double idCounter
                {
                    get
                    {
                        return p2.p22.Material.idCounterk__BackingField;
                    }
                    set
                    {
                        p2.p22.Material.idCounterk__BackingField = value;
                    }
                }

                public extern Material(double id);

                public virtual double id
                {
                    get
                    {
                        return this.idk__BackingField;
                    }
                    set
                    {
                        this.idk__BackingField = value;
                    }
                }
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
                private double? massk__BackingField;
                private double[] positionk__BackingField;
                private double[] velocityk__BackingField;
                private double? anglek__BackingField;
                private double? angularVelocityk__BackingField;
                private double[] forcek__BackingField;
                private double? angularForcek__BackingField;
                private bool? fixedRotationk__BackingField;

                public double? mass
                {
                    get
                    {
                        return this.massk__BackingField;
                    }
                    set
                    {
                        this.massk__BackingField = value;
                    }
                }

                public double[] position
                {
                    get
                    {
                        return this.positionk__BackingField;
                    }
                    set
                    {
                        this.positionk__BackingField = value;
                    }
                }

                public double[] velocity
                {
                    get
                    {
                        return this.velocityk__BackingField;
                    }
                    set
                    {
                        this.velocityk__BackingField = value;
                    }
                }

                public double? angle
                {
                    get
                    {
                        return this.anglek__BackingField;
                    }
                    set
                    {
                        this.anglek__BackingField = value;
                    }
                }

                public double? angularVelocity
                {
                    get
                    {
                        return this.angularVelocityk__BackingField;
                    }
                    set
                    {
                        this.angularVelocityk__BackingField = value;
                    }
                }

                public double[] force
                {
                    get
                    {
                        return this.forcek__BackingField;
                    }
                    set
                    {
                        this.forcek__BackingField = value;
                    }
                }

                public double? angularForce
                {
                    get
                    {
                        return this.angularForcek__BackingField;
                    }
                    set
                    {
                        this.angularForcek__BackingField = value;
                    }
                }

                public bool? fixedRotation
                {
                    get
                    {
                        return this.fixedRotationk__BackingField;
                    }
                    set
                    {
                        this.fixedRotationk__BackingField = value;
                    }
                }

                public BodyOptions() : base()
                {
                    
                }
            }

            public class Body : p2.p22.EventEmitter
            {
                private p2.p22.Body.sleepyEventConfig sleepyEventk__BackingField;
                private p2.p22.Body.sleepEventConfig sleepEventk__BackingField;
                private p2.p22.Body.wakeUpEventConfig wakeUpEventk__BackingField;
                private static double DYNAMICk__BackingField;
                private static double STATICk__BackingField;
                private static double KINEMATICk__BackingField;
                private static double AWAKEk__BackingField;
                private static double SLEEPYk__BackingField;
                private static double SLEEPINGk__BackingField;
                private double idk__BackingField;
                private p2.p22.World worldk__BackingField;
                private p2.p22.Shape[] shapesk__BackingField;
                private double massk__BackingField;
                private double invMassk__BackingField;
                private double inertiak__BackingField;
                private double invInertiak__BackingField;
                private double invMassSolvek__BackingField;
                private double invInertiaSolvek__BackingField;
                private double fixedRotationk__BackingField;
                private double[] positionk__BackingField;
                private double[] interpolatedPositionk__BackingField;
                private double interpolatedAnglek__BackingField;
                private double[] previousPositionk__BackingField;
                private double previousAnglek__BackingField;
                private double[] velocityk__BackingField;
                private double[] vlambdak__BackingField;
                private double[] wlambdak__BackingField;
                private double anglek__BackingField;
                private double angularVelocityk__BackingField;
                private double[] forcek__BackingField;
                private double angularForcek__BackingField;
                private double dampingk__BackingField;
                private double angularDampingk__BackingField;
                private double typek__BackingField;
                private double boundingRadiusk__BackingField;
                private p2.p22.AABB aabbk__BackingField;
                private bool aabbNeedsUpdatek__BackingField;
                private bool allowSleepk__BackingField;
                private bool wantsToSleepk__BackingField;
                private double sleepStatek__BackingField;
                private double sleepSpeedLimitk__BackingField;
                private double sleepTimeLimitk__BackingField;
                private double gravityScalek__BackingField;
                private bool collisionResponsek__BackingField;

                public virtual p2.p22.Body.sleepyEventConfig sleepyEvent
                {
                    get
                    {
                        return this.sleepyEventk__BackingField;
                    }
                    set
                    {
                        this.sleepyEventk__BackingField = value;
                    }
                }

                public virtual p2.p22.Body.sleepEventConfig sleepEvent
                {
                    get
                    {
                        return this.sleepEventk__BackingField;
                    }
                    set
                    {
                        this.sleepEventk__BackingField = value;
                    }
                }

                public virtual p2.p22.Body.wakeUpEventConfig wakeUpEvent
                {
                    get
                    {
                        return this.wakeUpEventk__BackingField;
                    }
                    set
                    {
                        this.wakeUpEventk__BackingField = value;
                    }
                }

                public static double DYNAMIC
                {
                    get
                    {
                        return p2.p22.Body.DYNAMICk__BackingField;
                    }
                    set
                    {
                        p2.p22.Body.DYNAMICk__BackingField = value;
                    }
                }

                public static double STATIC
                {
                    get
                    {
                        return p2.p22.Body.STATICk__BackingField;
                    }
                    set
                    {
                        p2.p22.Body.STATICk__BackingField = value;
                    }
                }

                public static double KINEMATIC
                {
                    get
                    {
                        return p2.p22.Body.KINEMATICk__BackingField;
                    }
                    set
                    {
                        p2.p22.Body.KINEMATICk__BackingField = value;
                    }
                }

                public static double AWAKE
                {
                    get
                    {
                        return p2.p22.Body.AWAKEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Body.AWAKEk__BackingField = value;
                    }
                }

                public static double SLEEPY
                {
                    get
                    {
                        return p2.p22.Body.SLEEPYk__BackingField;
                    }
                    set
                    {
                        p2.p22.Body.SLEEPYk__BackingField = value;
                    }
                }

                public static double SLEEPING
                {
                    get
                    {
                        return p2.p22.Body.SLEEPINGk__BackingField;
                    }
                    set
                    {
                        p2.p22.Body.SLEEPINGk__BackingField = value;
                    }
                }

                public extern Body();

                public extern Body(p2.p22.BodyOptions options);

                public virtual double id
                {
                    get
                    {
                        return this.idk__BackingField;
                    }
                    set
                    {
                        this.idk__BackingField = value;
                    }
                }

                public virtual p2.p22.World world
                {
                    get
                    {
                        return this.worldk__BackingField;
                    }
                    set
                    {
                        this.worldk__BackingField = value;
                    }
                }

                public virtual p2.p22.Shape[] shapes
                {
                    get
                    {
                        return this.shapesk__BackingField;
                    }
                    set
                    {
                        this.shapesk__BackingField = value;
                    }
                }

                public virtual double mass
                {
                    get
                    {
                        return this.massk__BackingField;
                    }
                    set
                    {
                        this.massk__BackingField = value;
                    }
                }

                public virtual double invMass
                {
                    get
                    {
                        return this.invMassk__BackingField;
                    }
                    set
                    {
                        this.invMassk__BackingField = value;
                    }
                }

                public virtual double inertia
                {
                    get
                    {
                        return this.inertiak__BackingField;
                    }
                    set
                    {
                        this.inertiak__BackingField = value;
                    }
                }

                public virtual double invInertia
                {
                    get
                    {
                        return this.invInertiak__BackingField;
                    }
                    set
                    {
                        this.invInertiak__BackingField = value;
                    }
                }

                public virtual double invMassSolve
                {
                    get
                    {
                        return this.invMassSolvek__BackingField;
                    }
                    set
                    {
                        this.invMassSolvek__BackingField = value;
                    }
                }

                public virtual double invInertiaSolve
                {
                    get
                    {
                        return this.invInertiaSolvek__BackingField;
                    }
                    set
                    {
                        this.invInertiaSolvek__BackingField = value;
                    }
                }

                public virtual double fixedRotation
                {
                    get
                    {
                        return this.fixedRotationk__BackingField;
                    }
                    set
                    {
                        this.fixedRotationk__BackingField = value;
                    }
                }

                public virtual double[] position
                {
                    get
                    {
                        return this.positionk__BackingField;
                    }
                    set
                    {
                        this.positionk__BackingField = value;
                    }
                }

                public virtual double[] interpolatedPosition
                {
                    get
                    {
                        return this.interpolatedPositionk__BackingField;
                    }
                    set
                    {
                        this.interpolatedPositionk__BackingField = value;
                    }
                }

                public virtual double interpolatedAngle
                {
                    get
                    {
                        return this.interpolatedAnglek__BackingField;
                    }
                    set
                    {
                        this.interpolatedAnglek__BackingField = value;
                    }
                }

                public virtual double[] previousPosition
                {
                    get
                    {
                        return this.previousPositionk__BackingField;
                    }
                    set
                    {
                        this.previousPositionk__BackingField = value;
                    }
                }

                public virtual double previousAngle
                {
                    get
                    {
                        return this.previousAnglek__BackingField;
                    }
                    set
                    {
                        this.previousAnglek__BackingField = value;
                    }
                }

                public virtual double[] velocity
                {
                    get
                    {
                        return this.velocityk__BackingField;
                    }
                    set
                    {
                        this.velocityk__BackingField = value;
                    }
                }

                public virtual double[] vlambda
                {
                    get
                    {
                        return this.vlambdak__BackingField;
                    }
                    set
                    {
                        this.vlambdak__BackingField = value;
                    }
                }

                public virtual double[] wlambda
                {
                    get
                    {
                        return this.wlambdak__BackingField;
                    }
                    set
                    {
                        this.wlambdak__BackingField = value;
                    }
                }

                public virtual double angle
                {
                    get
                    {
                        return this.anglek__BackingField;
                    }
                    set
                    {
                        this.anglek__BackingField = value;
                    }
                }

                public virtual double angularVelocity
                {
                    get
                    {
                        return this.angularVelocityk__BackingField;
                    }
                    set
                    {
                        this.angularVelocityk__BackingField = value;
                    }
                }

                public virtual double[] force
                {
                    get
                    {
                        return this.forcek__BackingField;
                    }
                    set
                    {
                        this.forcek__BackingField = value;
                    }
                }

                public virtual double angularForce
                {
                    get
                    {
                        return this.angularForcek__BackingField;
                    }
                    set
                    {
                        this.angularForcek__BackingField = value;
                    }
                }

                public virtual double damping
                {
                    get
                    {
                        return this.dampingk__BackingField;
                    }
                    set
                    {
                        this.dampingk__BackingField = value;
                    }
                }

                public virtual double angularDamping
                {
                    get
                    {
                        return this.angularDampingk__BackingField;
                    }
                    set
                    {
                        this.angularDampingk__BackingField = value;
                    }
                }

                public virtual double type
                {
                    get
                    {
                        return this.typek__BackingField;
                    }
                    set
                    {
                        this.typek__BackingField = value;
                    }
                }

                public virtual double boundingRadius
                {
                    get
                    {
                        return this.boundingRadiusk__BackingField;
                    }
                    set
                    {
                        this.boundingRadiusk__BackingField = value;
                    }
                }

                public virtual p2.p22.AABB aabb
                {
                    get
                    {
                        return this.aabbk__BackingField;
                    }
                    set
                    {
                        this.aabbk__BackingField = value;
                    }
                }

                public virtual bool aabbNeedsUpdate
                {
                    get
                    {
                        return this.aabbNeedsUpdatek__BackingField;
                    }
                    set
                    {
                        this.aabbNeedsUpdatek__BackingField = value;
                    }
                }

                public virtual bool allowSleep
                {
                    get
                    {
                        return this.allowSleepk__BackingField;
                    }
                    set
                    {
                        this.allowSleepk__BackingField = value;
                    }
                }

                public virtual bool wantsToSleep
                {
                    get
                    {
                        return this.wantsToSleepk__BackingField;
                    }
                    set
                    {
                        this.wantsToSleepk__BackingField = value;
                    }
                }

                public virtual double sleepState
                {
                    get
                    {
                        return this.sleepStatek__BackingField;
                    }
                    set
                    {
                        this.sleepStatek__BackingField = value;
                    }
                }

                public virtual double sleepSpeedLimit
                {
                    get
                    {
                        return this.sleepSpeedLimitk__BackingField;
                    }
                    set
                    {
                        this.sleepSpeedLimitk__BackingField = value;
                    }
                }

                public virtual double sleepTimeLimit
                {
                    get
                    {
                        return this.sleepTimeLimitk__BackingField;
                    }
                    set
                    {
                        this.sleepTimeLimitk__BackingField = value;
                    }
                }

                public virtual double gravityScale
                {
                    get
                    {
                        return this.gravityScalek__BackingField;
                    }
                    set
                    {
                        this.gravityScalek__BackingField = value;
                    }
                }

                public virtual bool collisionResponse
                {
                    get
                    {
                        return this.collisionResponsek__BackingField;
                    }
                    set
                    {
                        this.collisionResponsek__BackingField = value;
                    }
                }

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
                    private string typek__BackingField;

                    public string type
                    {
                        get
                        {
                            return this.typek__BackingField;
                        }
                        set
                        {
                            this.typek__BackingField = value;
                        }
                    }

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
                    private string typek__BackingField;

                    public string type
                    {
                        get
                        {
                            return this.typek__BackingField;
                        }
                        set
                        {
                            this.typek__BackingField = value;
                        }
                    }

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
                    private string typek__BackingField;

                    public string type
                    {
                        get
                        {
                            return this.typek__BackingField;
                        }
                        set
                        {
                            this.typek__BackingField = value;
                        }
                    }

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
                    private bool? optimalDecompk__BackingField;
                    private bool? skipSimpleCheckk__BackingField;
                    private object removeCollinearPointsk__BackingField;

                    public bool? optimalDecomp
                    {
                        get
                        {
                            return this.optimalDecompk__BackingField;
                        }
                        set
                        {
                            this.optimalDecompk__BackingField = value;
                        }
                    }

                    public bool? skipSimpleCheck
                    {
                        get
                        {
                            return this.skipSimpleCheckk__BackingField;
                        }
                        set
                        {
                            this.skipSimpleCheckk__BackingField = value;
                        }
                    }

                    public object removeCollinearPoints
                    {
                        get
                        {
                            return this.removeCollinearPointsk__BackingField;
                        }
                        set
                        {
                            this.removeCollinearPointsk__BackingField = value;
                        }
                    }

                    public fromPolygonConfig() : base()
                    {
                        
                    }
                }
            }

            public class Spring : IObject
            {
                private double stiffnessk__BackingField;
                private double dampingk__BackingField;
                private p2.p22.Body bodyAk__BackingField;
                private p2.p22.Body bodyBk__BackingField;

                public extern Spring(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern Spring(p2.p22.Body bodyA, p2.p22.Body bodyB, p2.p22.Spring.Config options);

                public virtual double stiffness
                {
                    get
                    {
                        return this.stiffnessk__BackingField;
                    }
                    set
                    {
                        this.stiffnessk__BackingField = value;
                    }
                }

                public virtual double damping
                {
                    get
                    {
                        return this.dampingk__BackingField;
                    }
                    set
                    {
                        this.dampingk__BackingField = value;
                    }
                }

                public virtual p2.p22.Body bodyA
                {
                    get
                    {
                        return this.bodyAk__BackingField;
                    }
                    set
                    {
                        this.bodyAk__BackingField = value;
                    }
                }

                public virtual p2.p22.Body bodyB
                {
                    get
                    {
                        return this.bodyBk__BackingField;
                    }
                    set
                    {
                        this.bodyBk__BackingField = value;
                    }
                }

                public virtual extern void applyForce();

                [ObjectLiteral]
                public class Config : IObject
                {
                    private double? stiffnessk__BackingField;
                    private double? dampingk__BackingField;
                    private double[] localAnchorAk__BackingField;
                    private double[] localAnchorBk__BackingField;
                    private double[] worldAnchorAk__BackingField;
                    private double[] worldAnchorBk__BackingField;

                    public double? stiffness
                    {
                        get
                        {
                            return this.stiffnessk__BackingField;
                        }
                        set
                        {
                            this.stiffnessk__BackingField = value;
                        }
                    }

                    public double? damping
                    {
                        get
                        {
                            return this.dampingk__BackingField;
                        }
                        set
                        {
                            this.dampingk__BackingField = value;
                        }
                    }

                    public double[] localAnchorA
                    {
                        get
                        {
                            return this.localAnchorAk__BackingField;
                        }
                        set
                        {
                            this.localAnchorAk__BackingField = value;
                        }
                    }

                    public double[] localAnchorB
                    {
                        get
                        {
                            return this.localAnchorBk__BackingField;
                        }
                        set
                        {
                            this.localAnchorBk__BackingField = value;
                        }
                    }

                    public double[] worldAnchorA
                    {
                        get
                        {
                            return this.worldAnchorAk__BackingField;
                        }
                        set
                        {
                            this.worldAnchorAk__BackingField = value;
                        }
                    }

                    public double[] worldAnchorB
                    {
                        get
                        {
                            return this.worldAnchorBk__BackingField;
                        }
                        set
                        {
                            this.worldAnchorBk__BackingField = value;
                        }
                    }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class LinearSpring : p2.p22.Spring
            {
                private double[] localAnchorAk__BackingField;
                private double[] localAnchorBk__BackingField;
                private double restLengthk__BackingField;

                public extern LinearSpring(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public virtual double[] localAnchorA
                {
                    get
                    {
                        return this.localAnchorAk__BackingField;
                    }
                    set
                    {
                        this.localAnchorAk__BackingField = value;
                    }
                }

                public virtual double[] localAnchorB
                {
                    get
                    {
                        return this.localAnchorBk__BackingField;
                    }
                    set
                    {
                        this.localAnchorBk__BackingField = value;
                    }
                }

                public virtual double restLength
                {
                    get
                    {
                        return this.restLengthk__BackingField;
                    }
                    set
                    {
                        this.restLengthk__BackingField = value;
                    }
                }

                public virtual extern void setWorldAnchorA(double[] worldAnchorA);

                public virtual extern void setWorldAnchorB(double[] worldAnchorB);

                public virtual extern double[] getWorldAnchorA(double[] result);

                public virtual extern double[] getWorldAnchorB(double[] result);

                public override extern void applyForce();
            }

            public class RotationalSpring : p2.p22.Spring
            {
                private double restAnglek__BackingField;

                public extern RotationalSpring(p2.p22.Body bodyA, p2.p22.Body bodyB);

                public extern RotationalSpring(
                  p2.p22.Body bodyA,
                  p2.p22.Body bodyB,
                  p2.p22.RotationalSpring.Config options);

                public virtual double restAngle
                {
                    get
                    {
                        return this.restAnglek__BackingField;
                    }
                    set
                    {
                        this.restAnglek__BackingField = value;
                    }
                }

                [ObjectLiteral]
                public new class Config : IObject
                {
                    private double? restAnglek__BackingField;
                    private double? stiffnessk__BackingField;
                    private double? dampingk__BackingField;

                    public double? restAngle
                    {
                        get
                        {
                            return this.restAnglek__BackingField;
                        }
                        set
                        {
                            this.restAnglek__BackingField = value;
                        }
                    }

                    public double? stiffness
                    {
                        get
                        {
                            return this.stiffnessk__BackingField;
                        }
                        set
                        {
                            this.stiffnessk__BackingField = value;
                        }
                    }

                    public double? damping
                    {
                        get
                        {
                            return this.dampingk__BackingField;
                        }
                        set
                        {
                            this.dampingk__BackingField = value;
                        }
                    }

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
                private double? lengthk__BackingField;
                private double? radiusk__BackingField;

                public double? length
                {
                    get
                    {
                        return this.lengthk__BackingField;
                    }
                    set
                    {
                        this.lengthk__BackingField = value;
                    }
                }

                public double? radius
                {
                    get
                    {
                        return this.radiusk__BackingField;
                    }
                    set
                    {
                        this.radiusk__BackingField = value;
                    }
                }

                public CapsuleOptions() : base()
                {
                    
                }
            }

            public class Capsule : p2.p22.Shape
            {
                private double lengthk__BackingField;
                private double radiusk__BackingField;
                private static double idCounterk__BackingField;
                private static double CIRCLEk__BackingField;
                private static double PARTICLEk__BackingField;
                private static double PLANEk__BackingField;
                private static double CONVEXk__BackingField;
                private static double LINEk__BackingField;
                private static double BOXk__BackingField;
                private static double CAPSULEk__BackingField;
                private static double HEIGHTFIELDk__BackingField;

                public extern Capsule();

                public extern Capsule(p2.p22.CapsuleOptions options);

                public virtual double length
                {
                    get
                    {
                        return this.lengthk__BackingField;
                    }
                    set
                    {
                        this.lengthk__BackingField = value;
                    }
                }

                public virtual double radius
                {
                    get
                    {
                        return this.radiusk__BackingField;
                    }
                    set
                    {
                        this.radiusk__BackingField = value;
                    }
                }

                public new static double idCounter
                {
                    get
                    {
                        return p2.p22.Capsule.idCounterk__BackingField;
                    }
                    set
                    {
                        p2.p22.Capsule.idCounterk__BackingField = value;
                    }
                }

                public new static double CIRCLE
                {
                    get
                    {
                        return p2.p22.Capsule.CIRCLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Capsule.CIRCLEk__BackingField = value;
                    }
                }

                public new static double PARTICLE
                {
                    get
                    {
                        return p2.p22.Capsule.PARTICLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Capsule.PARTICLEk__BackingField = value;
                    }
                }

                public new static double PLANE
                {
                    get
                    {
                        return p2.p22.Capsule.PLANEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Capsule.PLANEk__BackingField = value;
                    }
                }

                public new static double CONVEX
                {
                    get
                    {
                        return p2.p22.Capsule.CONVEXk__BackingField;
                    }
                    set
                    {
                        p2.p22.Capsule.CONVEXk__BackingField = value;
                    }
                }

                public new static double LINE
                {
                    get
                    {
                        return p2.p22.Capsule.LINEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Capsule.LINEk__BackingField = value;
                    }
                }

                public new static double BOX
                {
                    get
                    {
                        return p2.p22.Capsule.BOXk__BackingField;
                    }
                    set
                    {
                        p2.p22.Capsule.BOXk__BackingField = value;
                    }
                }

                public new static double CAPSULE
                {
                    get
                    {
                        return p2.p22.Capsule.CAPSULEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Capsule.CAPSULEk__BackingField = value;
                    }
                }

                public new static double HEIGHTFIELD
                {
                    get
                    {
                        return p2.p22.Capsule.HEIGHTFIELDk__BackingField;
                    }
                    set
                    {
                        p2.p22.Capsule.HEIGHTFIELDk__BackingField = value;
                    }
                }
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
                private double? radiusk__BackingField;

                public double? radius
                {
                    get
                    {
                        return this.radiusk__BackingField;
                    }
                    set
                    {
                        this.radiusk__BackingField = value;
                    }
                }

                public CircleOptions() : base()
                {
                    
                }
            }

            public class Circle : p2.p22.Shape
            {
                private double radiusk__BackingField;
                private static double idCounterk__BackingField;
                private static double CIRCLEk__BackingField;
                private static double PARTICLEk__BackingField;
                private static double PLANEk__BackingField;
                private static double CONVEXk__BackingField;
                private static double LINEk__BackingField;
                private static double BOXk__BackingField;
                private static double CAPSULEk__BackingField;
                private static double HEIGHTFIELDk__BackingField;

                public extern Circle();

                public extern Circle(p2.p22.CircleOptions options);

                public virtual double radius
                {
                    get
                    {
                        return this.radiusk__BackingField;
                    }
                    set
                    {
                        this.radiusk__BackingField = value;
                    }
                }

                public new static double idCounter
                {
                    get
                    {
                        return p2.p22.Circle.idCounterk__BackingField;
                    }
                    set
                    {
                        p2.p22.Circle.idCounterk__BackingField = value;
                    }
                }

                public new static double CIRCLE
                {
                    get
                    {
                        return p2.p22.Circle.CIRCLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Circle.CIRCLEk__BackingField = value;
                    }
                }

                public new static double PARTICLE
                {
                    get
                    {
                        return p2.p22.Circle.PARTICLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Circle.PARTICLEk__BackingField = value;
                    }
                }

                public new static double PLANE
                {
                    get
                    {
                        return p2.p22.Circle.PLANEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Circle.PLANEk__BackingField = value;
                    }
                }

                public new static double CONVEX
                {
                    get
                    {
                        return p2.p22.Circle.CONVEXk__BackingField;
                    }
                    set
                    {
                        p2.p22.Circle.CONVEXk__BackingField = value;
                    }
                }

                public new static double LINE
                {
                    get
                    {
                        return p2.p22.Circle.LINEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Circle.LINEk__BackingField = value;
                    }
                }

                public new static double BOX
                {
                    get
                    {
                        return p2.p22.Circle.BOXk__BackingField;
                    }
                    set
                    {
                        p2.p22.Circle.BOXk__BackingField = value;
                    }
                }

                public new static double CAPSULE
                {
                    get
                    {
                        return p2.p22.Circle.CAPSULEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Circle.CAPSULEk__BackingField = value;
                    }
                }

                public new static double HEIGHTFIELD
                {
                    get
                    {
                        return p2.p22.Circle.HEIGHTFIELDk__BackingField;
                    }
                    set
                    {
                        p2.p22.Circle.HEIGHTFIELDk__BackingField = value;
                    }
                }
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
                private es5.ArrayLike<double>[] verticesk__BackingField;
                private es5.ArrayLike<double>[] axesk__BackingField;

                public es5.ArrayLike<double>[] vertices
                {
                    get
                    {
                        return this.verticesk__BackingField;
                    }
                    set
                    {
                        this.verticesk__BackingField = value;
                    }
                }

                public es5.ArrayLike<double>[] axes
                {
                    get
                    {
                        return this.axesk__BackingField;
                    }
                    set
                    {
                        this.axesk__BackingField = value;
                    }
                }

                public ConvexOptions() : base()
                {
                    
                }
            }

            public class Convex : p2.p22.Shape
            {
                private double[][] verticesk__BackingField;
                private double[][] axesk__BackingField;
                private double[] centerOfMassk__BackingField;
                private double[] trianglesk__BackingField;
                private double boundingRadiusk__BackingField;
                private static double idCounterk__BackingField;
                private static double CIRCLEk__BackingField;
                private static double PARTICLEk__BackingField;
                private static double PLANEk__BackingField;
                private static double CONVEXk__BackingField;
                private static double LINEk__BackingField;
                private static double BOXk__BackingField;
                private static double CAPSULEk__BackingField;
                private static double HEIGHTFIELDk__BackingField;

                public static extern double triangleArea(double[] a, double[] b, double[] c);

                public extern Convex();

                public extern Convex(p2.p22.ConvexOptions options);

                public virtual double[][] vertices
                {
                    get
                    {
                        return this.verticesk__BackingField;
                    }
                    set
                    {
                        this.verticesk__BackingField = value;
                    }
                }

                public virtual double[][] axes
                {
                    get
                    {
                        return this.axesk__BackingField;
                    }
                    set
                    {
                        this.axesk__BackingField = value;
                    }
                }

                public virtual double[] centerOfMass
                {
                    get
                    {
                        return this.centerOfMassk__BackingField;
                    }
                    set
                    {
                        this.centerOfMassk__BackingField = value;
                    }
                }

                public virtual double[] triangles
                {
                    get
                    {
                        return this.trianglesk__BackingField;
                    }
                    set
                    {
                        this.trianglesk__BackingField = value;
                    }
                }

                public override double boundingRadius
                {
                    get
                    {
                        return this.boundingRadiusk__BackingField;
                    }
                    set
                    {
                        this.boundingRadiusk__BackingField = value;
                    }
                }

                public virtual extern void projectOntoLocalAxis(double[] localAxis, double[] result);

                public virtual extern void projectOntoWorldAxis(
                  double[] localAxis,
                  double[] shapeOffset,
                  double shapeAngle,
                  double[] result);

                public virtual extern void updateCenterOfMass();

                public new static double idCounter
                {
                    get
                    {
                        return p2.p22.Convex.idCounterk__BackingField;
                    }
                    set
                    {
                        p2.p22.Convex.idCounterk__BackingField = value;
                    }
                }

                public new static double CIRCLE
                {
                    get
                    {
                        return p2.p22.Convex.CIRCLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Convex.CIRCLEk__BackingField = value;
                    }
                }

                public new static double PARTICLE
                {
                    get
                    {
                        return p2.p22.Convex.PARTICLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Convex.PARTICLEk__BackingField = value;
                    }
                }

                public new static double PLANE
                {
                    get
                    {
                        return p2.p22.Convex.PLANEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Convex.PLANEk__BackingField = value;
                    }
                }

                public new static double CONVEX
                {
                    get
                    {
                        return p2.p22.Convex.CONVEXk__BackingField;
                    }
                    set
                    {
                        p2.p22.Convex.CONVEXk__BackingField = value;
                    }
                }

                public new static double LINE
                {
                    get
                    {
                        return p2.p22.Convex.LINEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Convex.LINEk__BackingField = value;
                    }
                }

                public new static double BOX
                {
                    get
                    {
                        return p2.p22.Convex.BOXk__BackingField;
                    }
                    set
                    {
                        p2.p22.Convex.BOXk__BackingField = value;
                    }
                }

                public new static double CAPSULE
                {
                    get
                    {
                        return p2.p22.Convex.CAPSULEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Convex.CAPSULEk__BackingField = value;
                    }
                }

                public new static double HEIGHTFIELD
                {
                    get
                    {
                        return p2.p22.Convex.HEIGHTFIELDk__BackingField;
                    }
                    set
                    {
                        p2.p22.Convex.HEIGHTFIELDk__BackingField = value;
                    }
                }
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
                private double[] heightsk__BackingField;
                private double? minValuek__BackingField;
                private double? maxValuek__BackingField;
                private double? elementWidthk__BackingField;

                public double[] heights
                {
                    get
                    {
                        return this.heightsk__BackingField;
                    }
                    set
                    {
                        this.heightsk__BackingField = value;
                    }
                }

                public double? minValue
                {
                    get
                    {
                        return this.minValuek__BackingField;
                    }
                    set
                    {
                        this.minValuek__BackingField = value;
                    }
                }

                public double? maxValue
                {
                    get
                    {
                        return this.maxValuek__BackingField;
                    }
                    set
                    {
                        this.maxValuek__BackingField = value;
                    }
                }

                public double? elementWidth
                {
                    get
                    {
                        return this.elementWidthk__BackingField;
                    }
                    set
                    {
                        this.elementWidthk__BackingField = value;
                    }
                }

                public HeightfieldOptions() : base()
                {
                    
                }
            }

            public class Heightfield : p2.p22.Shape
            {
                private double[] datak__BackingField;
                private double maxValuek__BackingField;
                private double minValuek__BackingField;
                private double elementWidthk__BackingField;
                private static double idCounterk__BackingField;
                private static double CIRCLEk__BackingField;
                private static double PARTICLEk__BackingField;
                private static double PLANEk__BackingField;
                private static double CONVEXk__BackingField;
                private static double LINEk__BackingField;
                private static double BOXk__BackingField;
                private static double CAPSULEk__BackingField;
                private static double HEIGHTFIELDk__BackingField;

                public extern Heightfield();

                public extern Heightfield(p2.p22.HeightfieldOptions options);

                public virtual double[] data
                {
                    get
                    {
                        return this.datak__BackingField;
                    }
                    set
                    {
                        this.datak__BackingField = value;
                    }
                }

                public virtual double maxValue
                {
                    get
                    {
                        return this.maxValuek__BackingField;
                    }
                    set
                    {
                        this.maxValuek__BackingField = value;
                    }
                }

                public virtual double minValue
                {
                    get
                    {
                        return this.minValuek__BackingField;
                    }
                    set
                    {
                        this.minValuek__BackingField = value;
                    }
                }

                public virtual double elementWidth
                {
                    get
                    {
                        return this.elementWidthk__BackingField;
                    }
                    set
                    {
                        this.elementWidthk__BackingField = value;
                    }
                }

                public new static double idCounter
                {
                    get
                    {
                        return p2.p22.Heightfield.idCounterk__BackingField;
                    }
                    set
                    {
                        p2.p22.Heightfield.idCounterk__BackingField = value;
                    }
                }

                public new static double CIRCLE
                {
                    get
                    {
                        return p2.p22.Heightfield.CIRCLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Heightfield.CIRCLEk__BackingField = value;
                    }
                }

                public new static double PARTICLE
                {
                    get
                    {
                        return p2.p22.Heightfield.PARTICLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Heightfield.PARTICLEk__BackingField = value;
                    }
                }

                public new static double PLANE
                {
                    get
                    {
                        return p2.p22.Heightfield.PLANEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Heightfield.PLANEk__BackingField = value;
                    }
                }

                public new static double CONVEX
                {
                    get
                    {
                        return p2.p22.Heightfield.CONVEXk__BackingField;
                    }
                    set
                    {
                        p2.p22.Heightfield.CONVEXk__BackingField = value;
                    }
                }

                public new static double LINE
                {
                    get
                    {
                        return p2.p22.Heightfield.LINEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Heightfield.LINEk__BackingField = value;
                    }
                }

                public new static double BOX
                {
                    get
                    {
                        return p2.p22.Heightfield.BOXk__BackingField;
                    }
                    set
                    {
                        p2.p22.Heightfield.BOXk__BackingField = value;
                    }
                }

                public new static double CAPSULE
                {
                    get
                    {
                        return p2.p22.Heightfield.CAPSULEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Heightfield.CAPSULEk__BackingField = value;
                    }
                }

                public new static double HEIGHTFIELD
                {
                    get
                    {
                        return p2.p22.Heightfield.HEIGHTFIELDk__BackingField;
                    }
                    set
                    {
                        p2.p22.Heightfield.HEIGHTFIELDk__BackingField = value;
                    }
                }
            }

            [IgnoreCast]
            [ObjectLiteral]
            [FormerInterface]
            public class SharedShapeOptions : IObject
            {
                private double[] positionk__BackingField;
                private double? anglek__BackingField;
                private double? collisionGroupk__BackingField;
                private bool? collisionResponsek__BackingField;
                private double? collisionMaskk__BackingField;
                private bool? sensork__BackingField;

                public double[] position
                {
                    get
                    {
                        return this.positionk__BackingField;
                    }
                    set
                    {
                        this.positionk__BackingField = value;
                    }
                }

                public double? angle
                {
                    get
                    {
                        return this.anglek__BackingField;
                    }
                    set
                    {
                        this.anglek__BackingField = value;
                    }
                }

                public double? collisionGroup
                {
                    get
                    {
                        return this.collisionGroupk__BackingField;
                    }
                    set
                    {
                        this.collisionGroupk__BackingField = value;
                    }
                }

                public bool? collisionResponse
                {
                    get
                    {
                        return this.collisionResponsek__BackingField;
                    }
                    set
                    {
                        this.collisionResponsek__BackingField = value;
                    }
                }

                public double? collisionMask
                {
                    get
                    {
                        return this.collisionMaskk__BackingField;
                    }
                    set
                    {
                        this.collisionMaskk__BackingField = value;
                    }
                }

                public bool? sensor
                {
                    get
                    {
                        return this.sensork__BackingField;
                    }
                    set
                    {
                        this.sensork__BackingField = value;
                    }
                }

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
                private double? typek__BackingField;

                public double? type
                {
                    get
                    {
                        return this.typek__BackingField;
                    }
                    set
                    {
                        this.typek__BackingField = value;
                    }
                }

                public ShapeOptions() : base()
                {
                    
                }
            }

            public class Shape : IObject
            {
                private static double idCounterk__BackingField;
                private static double CIRCLEk__BackingField;
                private static double PARTICLEk__BackingField;
                private static double PLANEk__BackingField;
                private static double CONVEXk__BackingField;
                private static double LINEk__BackingField;
                private static double BOXk__BackingField;
                private static double CAPSULEk__BackingField;
                private static double HEIGHTFIELDk__BackingField;
                private double typek__BackingField;
                private double idk__BackingField;
                private double[] positionk__BackingField;
                private double anglek__BackingField;
                private double boundingRadiusk__BackingField;
                private double collisionGroupk__BackingField;
                private bool collisionResponsek__BackingField;
                private double collisionMaskk__BackingField;
                private p2.p22.Material materialk__BackingField;
                private double areak__BackingField;
                private bool sensork__BackingField;

                public static double idCounter
                {
                    get
                    {
                        return p2.p22.Shape.idCounterk__BackingField;
                    }
                    set
                    {
                        p2.p22.Shape.idCounterk__BackingField = value;
                    }
                }

                public static double CIRCLE
                {
                    get
                    {
                        return p2.p22.Shape.CIRCLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Shape.CIRCLEk__BackingField = value;
                    }
                }

                public static double PARTICLE
                {
                    get
                    {
                        return p2.p22.Shape.PARTICLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Shape.PARTICLEk__BackingField = value;
                    }
                }

                public static double PLANE
                {
                    get
                    {
                        return p2.p22.Shape.PLANEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Shape.PLANEk__BackingField = value;
                    }
                }

                public static double CONVEX
                {
                    get
                    {
                        return p2.p22.Shape.CONVEXk__BackingField;
                    }
                    set
                    {
                        p2.p22.Shape.CONVEXk__BackingField = value;
                    }
                }

                public static double LINE
                {
                    get
                    {
                        return p2.p22.Shape.LINEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Shape.LINEk__BackingField = value;
                    }
                }

                public static double BOX
                {
                    get
                    {
                        return p2.p22.Shape.BOXk__BackingField;
                    }
                    set
                    {
                        p2.p22.Shape.BOXk__BackingField = value;
                    }
                }

                public static double CAPSULE
                {
                    get
                    {
                        return p2.p22.Shape.CAPSULEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Shape.CAPSULEk__BackingField = value;
                    }
                }

                public static double HEIGHTFIELD
                {
                    get
                    {
                        return p2.p22.Shape.HEIGHTFIELDk__BackingField;
                    }
                    set
                    {
                        p2.p22.Shape.HEIGHTFIELDk__BackingField = value;
                    }
                }

                public extern Shape();

                public extern Shape(p2.p22.ShapeOptions options);

                public virtual double type
                {
                    get
                    {
                        return this.typek__BackingField;
                    }
                    set
                    {
                        this.typek__BackingField = value;
                    }
                }

                public virtual double id
                {
                    get
                    {
                        return this.idk__BackingField;
                    }
                    set
                    {
                        this.idk__BackingField = value;
                    }
                }

                public virtual double[] position
                {
                    get
                    {
                        return this.positionk__BackingField;
                    }
                    set
                    {
                        this.positionk__BackingField = value;
                    }
                }

                public virtual double angle
                {
                    get
                    {
                        return this.anglek__BackingField;
                    }
                    set
                    {
                        this.anglek__BackingField = value;
                    }
                }

                public virtual double boundingRadius
                {
                    get
                    {
                        return this.boundingRadiusk__BackingField;
                    }
                    set
                    {
                        this.boundingRadiusk__BackingField = value;
                    }
                }

                public virtual double collisionGroup
                {
                    get
                    {
                        return this.collisionGroupk__BackingField;
                    }
                    set
                    {
                        this.collisionGroupk__BackingField = value;
                    }
                }

                public virtual bool collisionResponse
                {
                    get
                    {
                        return this.collisionResponsek__BackingField;
                    }
                    set
                    {
                        this.collisionResponsek__BackingField = value;
                    }
                }

                public virtual double collisionMask
                {
                    get
                    {
                        return this.collisionMaskk__BackingField;
                    }
                    set
                    {
                        this.collisionMaskk__BackingField = value;
                    }
                }

                public virtual p2.p22.Material material
                {
                    get
                    {
                        return this.materialk__BackingField;
                    }
                    set
                    {
                        this.materialk__BackingField = value;
                    }
                }

                public virtual double area
                {
                    get
                    {
                        return this.areak__BackingField;
                    }
                    set
                    {
                        this.areak__BackingField = value;
                    }
                }

                public virtual bool sensor
                {
                    get
                    {
                        return this.sensork__BackingField;
                    }
                    set
                    {
                        this.sensork__BackingField = value;
                    }
                }

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
                private double? lengthk__BackingField;

                public double? length
                {
                    get
                    {
                        return this.lengthk__BackingField;
                    }
                    set
                    {
                        this.lengthk__BackingField = value;
                    }
                }

                public LineOptions() : base()
                {
                    
                }
            }

            public class Line : p2.p22.Shape
            {
                private double lengthk__BackingField;
                private static double idCounterk__BackingField;
                private static double CIRCLEk__BackingField;
                private static double PARTICLEk__BackingField;
                private static double PLANEk__BackingField;
                private static double CONVEXk__BackingField;
                private static double LINEk__BackingField;
                private static double BOXk__BackingField;
                private static double CAPSULEk__BackingField;
                private static double HEIGHTFIELDk__BackingField;

                public extern Line();

                public extern Line(p2.p22.LineOptions options);

                public virtual double length
                {
                    get
                    {
                        return this.lengthk__BackingField;
                    }
                    set
                    {
                        this.lengthk__BackingField = value;
                    }
                }

                public new static double idCounter
                {
                    get
                    {
                        return p2.p22.Line.idCounterk__BackingField;
                    }
                    set
                    {
                        p2.p22.Line.idCounterk__BackingField = value;
                    }
                }

                public new static double CIRCLE
                {
                    get
                    {
                        return p2.p22.Line.CIRCLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Line.CIRCLEk__BackingField = value;
                    }
                }

                public new static double PARTICLE
                {
                    get
                    {
                        return p2.p22.Line.PARTICLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Line.PARTICLEk__BackingField = value;
                    }
                }

                public new static double PLANE
                {
                    get
                    {
                        return p2.p22.Line.PLANEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Line.PLANEk__BackingField = value;
                    }
                }

                public new static double CONVEX
                {
                    get
                    {
                        return p2.p22.Line.CONVEXk__BackingField;
                    }
                    set
                    {
                        p2.p22.Line.CONVEXk__BackingField = value;
                    }
                }

                public new static double LINE
                {
                    get
                    {
                        return p2.p22.Line.LINEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Line.LINEk__BackingField = value;
                    }
                }

                public new static double BOX
                {
                    get
                    {
                        return p2.p22.Line.BOXk__BackingField;
                    }
                    set
                    {
                        p2.p22.Line.BOXk__BackingField = value;
                    }
                }

                public new static double CAPSULE
                {
                    get
                    {
                        return p2.p22.Line.CAPSULEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Line.CAPSULEk__BackingField = value;
                    }
                }

                public new static double HEIGHTFIELD
                {
                    get
                    {
                        return p2.p22.Line.HEIGHTFIELDk__BackingField;
                    }
                    set
                    {
                        p2.p22.Line.HEIGHTFIELDk__BackingField = value;
                    }
                }
            }

            public class Particle : p2.p22.Shape
            {
                private static double idCounterk__BackingField;
                private static double CIRCLEk__BackingField;
                private static double PARTICLEk__BackingField;
                private static double PLANEk__BackingField;
                private static double CONVEXk__BackingField;
                private static double LINEk__BackingField;
                private static double BOXk__BackingField;
                private static double CAPSULEk__BackingField;
                private static double HEIGHTFIELDk__BackingField;

                public extern Particle();

                public extern Particle(p2.p22.SharedShapeOptions options);

                public new static double idCounter
                {
                    get
                    {
                        return p2.p22.Particle.idCounterk__BackingField;
                    }
                    set
                    {
                        p2.p22.Particle.idCounterk__BackingField = value;
                    }
                }

                public new static double CIRCLE
                {
                    get
                    {
                        return p2.p22.Particle.CIRCLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Particle.CIRCLEk__BackingField = value;
                    }
                }

                public new static double PARTICLE
                {
                    get
                    {
                        return p2.p22.Particle.PARTICLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Particle.PARTICLEk__BackingField = value;
                    }
                }

                public new static double PLANE
                {
                    get
                    {
                        return p2.p22.Particle.PLANEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Particle.PLANEk__BackingField = value;
                    }
                }

                public new static double CONVEX
                {
                    get
                    {
                        return p2.p22.Particle.CONVEXk__BackingField;
                    }
                    set
                    {
                        p2.p22.Particle.CONVEXk__BackingField = value;
                    }
                }

                public new static double LINE
                {
                    get
                    {
                        return p2.p22.Particle.LINEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Particle.LINEk__BackingField = value;
                    }
                }

                public new static double BOX
                {
                    get
                    {
                        return p2.p22.Particle.BOXk__BackingField;
                    }
                    set
                    {
                        p2.p22.Particle.BOXk__BackingField = value;
                    }
                }

                public new static double CAPSULE
                {
                    get
                    {
                        return p2.p22.Particle.CAPSULEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Particle.CAPSULEk__BackingField = value;
                    }
                }

                public new static double HEIGHTFIELD
                {
                    get
                    {
                        return p2.p22.Particle.HEIGHTFIELDk__BackingField;
                    }
                    set
                    {
                        p2.p22.Particle.HEIGHTFIELDk__BackingField = value;
                    }
                }
            }

            public class Plane : p2.p22.Shape
            {
                private static double idCounterk__BackingField;
                private static double CIRCLEk__BackingField;
                private static double PARTICLEk__BackingField;
                private static double PLANEk__BackingField;
                private static double CONVEXk__BackingField;
                private static double LINEk__BackingField;
                private static double BOXk__BackingField;
                private static double CAPSULEk__BackingField;
                private static double HEIGHTFIELDk__BackingField;

                public extern Plane();

                public extern Plane(p2.p22.SharedShapeOptions options);

                public new static double idCounter
                {
                    get
                    {
                        return p2.p22.Plane.idCounterk__BackingField;
                    }
                    set
                    {
                        p2.p22.Plane.idCounterk__BackingField = value;
                    }
                }

                public new static double CIRCLE
                {
                    get
                    {
                        return p2.p22.Plane.CIRCLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Plane.CIRCLEk__BackingField = value;
                    }
                }

                public new static double PARTICLE
                {
                    get
                    {
                        return p2.p22.Plane.PARTICLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Plane.PARTICLEk__BackingField = value;
                    }
                }

                public new static double PLANE
                {
                    get
                    {
                        return p2.p22.Plane.PLANEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Plane.PLANEk__BackingField = value;
                    }
                }

                public new static double CONVEX
                {
                    get
                    {
                        return p2.p22.Plane.CONVEXk__BackingField;
                    }
                    set
                    {
                        p2.p22.Plane.CONVEXk__BackingField = value;
                    }
                }

                public new static double LINE
                {
                    get
                    {
                        return p2.p22.Plane.LINEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Plane.LINEk__BackingField = value;
                    }
                }

                public new static double BOX
                {
                    get
                    {
                        return p2.p22.Plane.BOXk__BackingField;
                    }
                    set
                    {
                        p2.p22.Plane.BOXk__BackingField = value;
                    }
                }

                public new static double CAPSULE
                {
                    get
                    {
                        return p2.p22.Plane.CAPSULEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Plane.CAPSULEk__BackingField = value;
                    }
                }

                public new static double HEIGHTFIELD
                {
                    get
                    {
                        return p2.p22.Plane.HEIGHTFIELDk__BackingField;
                    }
                    set
                    {
                        p2.p22.Plane.HEIGHTFIELDk__BackingField = value;
                    }
                }
            }

            [IgnoreCast]
            [ObjectLiteral]
            [FormerInterface]
            public class BoxOptions : IObject
            {
                private double? widthk__BackingField;
                private double? heightk__BackingField;

                public double? width
                {
                    get
                    {
                        return this.widthk__BackingField;
                    }
                    set
                    {
                        this.widthk__BackingField = value;
                    }
                }

                public double? height
                {
                    get
                    {
                        return this.heightk__BackingField;
                    }
                    set
                    {
                        this.heightk__BackingField = value;
                    }
                }

                public BoxOptions() : base()
                {
                    
                }
            }

            public class Box : p2.p22.Shape
            {
                private double widthk__BackingField;
                private double heightk__BackingField;
                private static double idCounterk__BackingField;
                private static double CIRCLEk__BackingField;
                private static double PARTICLEk__BackingField;
                private static double PLANEk__BackingField;
                private static double CONVEXk__BackingField;
                private static double LINEk__BackingField;
                private static double BOXk__BackingField;
                private static double CAPSULEk__BackingField;
                private static double HEIGHTFIELDk__BackingField;

                public extern Box();

                public extern Box(p2.p22.BoxOptions options);

                public virtual double width
                {
                    get
                    {
                        return this.widthk__BackingField;
                    }
                    set
                    {
                        this.widthk__BackingField = value;
                    }
                }

                public virtual double height
                {
                    get
                    {
                        return this.heightk__BackingField;
                    }
                    set
                    {
                        this.heightk__BackingField = value;
                    }
                }

                public new static double idCounter
                {
                    get
                    {
                        return p2.p22.Box.idCounterk__BackingField;
                    }
                    set
                    {
                        p2.p22.Box.idCounterk__BackingField = value;
                    }
                }

                public new static double CIRCLE
                {
                    get
                    {
                        return p2.p22.Box.CIRCLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Box.CIRCLEk__BackingField = value;
                    }
                }

                public new static double PARTICLE
                {
                    get
                    {
                        return p2.p22.Box.PARTICLEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Box.PARTICLEk__BackingField = value;
                    }
                }

                public new static double PLANE
                {
                    get
                    {
                        return p2.p22.Box.PLANEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Box.PLANEk__BackingField = value;
                    }
                }

                public new static double CONVEX
                {
                    get
                    {
                        return p2.p22.Box.CONVEXk__BackingField;
                    }
                    set
                    {
                        p2.p22.Box.CONVEXk__BackingField = value;
                    }
                }

                public new static double LINE
                {
                    get
                    {
                        return p2.p22.Box.LINEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Box.LINEk__BackingField = value;
                    }
                }

                public new static double BOX
                {
                    get
                    {
                        return p2.p22.Box.BOXk__BackingField;
                    }
                    set
                    {
                        p2.p22.Box.BOXk__BackingField = value;
                    }
                }

                public new static double CAPSULE
                {
                    get
                    {
                        return p2.p22.Box.CAPSULEk__BackingField;
                    }
                    set
                    {
                        p2.p22.Box.CAPSULEk__BackingField = value;
                    }
                }

                public new static double HEIGHTFIELD
                {
                    get
                    {
                        return p2.p22.Box.HEIGHTFIELDk__BackingField;
                    }
                    set
                    {
                        p2.p22.Box.HEIGHTFIELDk__BackingField = value;
                    }
                }
            }

            public class Solver : p2.p22.EventEmitter
            {
                private static double GSk__BackingField;
                private static double ISLANDk__BackingField;
                private double typek__BackingField;
                private p2.p22.Equation[] equationsk__BackingField;
                private p2.p22.Equation equationSortFunctionk__BackingField;

                public static double GS
                {
                    get
                    {
                        return p2.p22.Solver.GSk__BackingField;
                    }
                    set
                    {
                        p2.p22.Solver.GSk__BackingField = value;
                    }
                }

                public static double ISLAND
                {
                    get
                    {
                        return p2.p22.Solver.ISLANDk__BackingField;
                    }
                    set
                    {
                        p2.p22.Solver.ISLANDk__BackingField = value;
                    }
                }

                public extern Solver();

                public extern Solver(object options);

                public extern Solver(object options, double type);

                public virtual double type
                {
                    get
                    {
                        return this.typek__BackingField;
                    }
                    set
                    {
                        this.typek__BackingField = value;
                    }
                }

                public virtual p2.p22.Equation[] equations
                {
                    get
                    {
                        return this.equationsk__BackingField;
                    }
                    set
                    {
                        this.equationsk__BackingField = value;
                    }
                }

                public virtual p2.p22.Equation equationSortFunction
                {
                    get
                    {
                        return this.equationSortFunctionk__BackingField;
                    }
                    set
                    {
                        this.equationSortFunctionk__BackingField = value;
                    }
                }

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
                private double iterationsk__BackingField;
                private double tolerancek__BackingField;
                private bool useZeroRHSk__BackingField;
                private double frictionIterationsk__BackingField;
                private double usedIterationsk__BackingField;
                private static double GSk__BackingField;
                private static double ISLANDk__BackingField;

                public extern GSSolver();

                public extern GSSolver(p2.p22.GSSolver.Config options);

                public virtual double iterations
                {
                    get
                    {
                        return this.iterationsk__BackingField;
                    }
                    set
                    {
                        this.iterationsk__BackingField = value;
                    }
                }

                public virtual double tolerance
                {
                    get
                    {
                        return this.tolerancek__BackingField;
                    }
                    set
                    {
                        this.tolerancek__BackingField = value;
                    }
                }

                public virtual bool useZeroRHS
                {
                    get
                    {
                        return this.useZeroRHSk__BackingField;
                    }
                    set
                    {
                        this.useZeroRHSk__BackingField = value;
                    }
                }

                public virtual double frictionIterations
                {
                    get
                    {
                        return this.frictionIterationsk__BackingField;
                    }
                    set
                    {
                        this.frictionIterationsk__BackingField = value;
                    }
                }

                public virtual double usedIterations
                {
                    get
                    {
                        return this.usedIterationsk__BackingField;
                    }
                    set
                    {
                        this.usedIterationsk__BackingField = value;
                    }
                }

                public override extern void solve(double h, p2.p22.World world);

                public new static double GS
                {
                    get
                    {
                        return p2.p22.GSSolver.GSk__BackingField;
                    }
                    set
                    {
                        p2.p22.GSSolver.GSk__BackingField = value;
                    }
                }

                public new static double ISLAND
                {
                    get
                    {
                        return p2.p22.GSSolver.ISLANDk__BackingField;
                    }
                    set
                    {
                        p2.p22.GSSolver.ISLANDk__BackingField = value;
                    }
                }

                [ObjectLiteral]
                public class Config : IObject
                {
                    private double? iterationsk__BackingField;
                    private double? tolerancek__BackingField;

                    public double? iterations
                    {
                        get
                        {
                            return this.iterationsk__BackingField;
                        }
                        set
                        {
                            this.iterationsk__BackingField = value;
                        }
                    }

                    public double? tolerance
                    {
                        get
                        {
                            return this.tolerancek__BackingField;
                        }
                        set
                        {
                            this.tolerancek__BackingField = value;
                        }
                    }

                    public Config() : base()
                    {
                        
                    }
                }
            }

            public class OverlapKeeper : IObject
            {
                private p2.p22.Shape shapeAk__BackingField;
                private p2.p22.Shape shapeBk__BackingField;
                private p2.p22.Body bodyAk__BackingField;
                private p2.p22.Body bodyBk__BackingField;

                public extern OverlapKeeper(
                  p2.p22.Body bodyA,
                  p2.p22.Shape shapeA,
                  p2.p22.Body bodyB,
                  p2.p22.Shape shapeB);

                public virtual p2.p22.Shape shapeA
                {
                    get
                    {
                        return this.shapeAk__BackingField;
                    }
                    set
                    {
                        this.shapeAk__BackingField = value;
                    }
                }

                public virtual p2.p22.Shape shapeB
                {
                    get
                    {
                        return this.shapeBk__BackingField;
                    }
                    set
                    {
                        this.shapeBk__BackingField = value;
                    }
                }

                public virtual p2.p22.Body bodyA
                {
                    get
                    {
                        return this.bodyAk__BackingField;
                    }
                    set
                    {
                        this.bodyAk__BackingField = value;
                    }
                }

                public virtual p2.p22.Body bodyB
                {
                    get
                    {
                        return this.bodyBk__BackingField;
                    }
                    set
                    {
                        this.bodyBk__BackingField = value;
                    }
                }

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
                private double[] datak__BackingField;
                private double[] keysk__BackingField;

                public virtual double[] data
                {
                    get
                    {
                        return this.datak__BackingField;
                    }
                    set
                    {
                        this.datak__BackingField = value;
                    }
                }

                public virtual double[] keys
                {
                    get
                    {
                        return this.keysk__BackingField;
                    }
                    set
                    {
                        this.keysk__BackingField = value;
                    }
                }

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
                private p2.p22.Equation[] equationsk__BackingField;
                private p2.p22.Body[] bodiesk__BackingField;

                public virtual p2.p22.Equation[] equations
                {
                    get
                    {
                        return this.equationsk__BackingField;
                    }
                    set
                    {
                        this.equationsk__BackingField = value;
                    }
                }

                public virtual p2.p22.Body[] bodies
                {
                    get
                    {
                        return this.bodiesk__BackingField;
                    }
                    set
                    {
                        this.bodiesk__BackingField = value;
                    }
                }

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
                private p2.p22.Equation[] equationsk__BackingField;
                private p2.p22.Island[] islandsk__BackingField;
                private p2.p22.IslandNode[] nodesk__BackingField;
                private static double GSk__BackingField;
                private static double ISLANDk__BackingField;

                public static extern p2.p22.IslandNode getUnvisitedNode(p2.p22.IslandNode[] nodes);

                public override p2.p22.Equation[] equations
                {
                    get
                    {
                        return this.equationsk__BackingField;
                    }
                    set
                    {
                        this.equationsk__BackingField = value;
                    }
                }

                public virtual p2.p22.Island[] islands
                {
                    get
                    {
                        return this.islandsk__BackingField;
                    }
                    set
                    {
                        this.islandsk__BackingField = value;
                    }
                }

                public virtual p2.p22.IslandNode[] nodes
                {
                    get
                    {
                        return this.nodesk__BackingField;
                    }
                    set
                    {
                        this.nodesk__BackingField = value;
                    }
                }

                public virtual extern void visit(
                  p2.p22.IslandNode node,
                  p2.p22.Body[] bds,
                  p2.p22.Equation[] eqs);

                public virtual extern void bfs(
                  p2.p22.IslandNode root,
                  p2.p22.Body[] bds,
                  p2.p22.Equation[] eqs);

                public virtual extern p2.p22.Island[] split(p2.p22.World world);

                public new static double GS
                {
                    get
                    {
                        return p2.p22.IslandManager.GSk__BackingField;
                    }
                    set
                    {
                        p2.p22.IslandManager.GSk__BackingField = value;
                    }
                }

                public new static double ISLAND
                {
                    get
                    {
                        return p2.p22.IslandManager.ISLANDk__BackingField;
                    }
                    set
                    {
                        p2.p22.IslandManager.ISLANDk__BackingField = value;
                    }
                }

                public IslandManager() : base()
                {
                    
                }
            }

            public class IslandNode : IObject
            {
                private p2.p22.Body bodyk__BackingField;
                private p2.p22.IslandNode[] neighborsk__BackingField;
                private p2.p22.Equation[] equationsk__BackingField;
                private bool visitedk__BackingField;

                public extern IslandNode(p2.p22.Body body);

                public virtual p2.p22.Body body
                {
                    get
                    {
                        return this.bodyk__BackingField;
                    }
                    set
                    {
                        this.bodyk__BackingField = value;
                    }
                }

                public virtual p2.p22.IslandNode[] neighbors
                {
                    get
                    {
                        return this.neighborsk__BackingField;
                    }
                    set
                    {
                        this.neighborsk__BackingField = value;
                    }
                }

                public virtual p2.p22.Equation[] equations
                {
                    get
                    {
                        return this.equationsk__BackingField;
                    }
                    set
                    {
                        this.equationsk__BackingField = value;
                    }
                }

                public virtual bool visited
                {
                    get
                    {
                        return this.visitedk__BackingField;
                    }
                    set
                    {
                        this.visitedk__BackingField = value;
                    }
                }

                public virtual extern void reset();
            }

            public class World : p2.p22.EventEmitter
            {
                private p2.p22.World.postStepEventConfig postStepEventk__BackingField;
                private p2.p22.World.addBodyEventConfig addBodyEventk__BackingField;
                private p2.p22.World.removeBodyEventConfig removeBodyEventk__BackingField;
                private p2.p22.World.addSpringEventConfig addSpringEventk__BackingField;
                private p2.p22.World.impactEventConfig impactEventk__BackingField;
                private p2.p22.World.postBroadphaseEventConfig postBroadphaseEventk__BackingField;
                private p2.p22.World.beginContactEventConfig beginContactEventk__BackingField;
                private p2.p22.World.endContactEventConfig endContactEventk__BackingField;
                private p2.p22.World.preSolveEventConfig preSolveEventk__BackingField;
                private static double NO_SLEEPINGk__BackingField;
                private static double BODY_SLEEPINGk__BackingField;
                private static double ISLAND_SLEEPINGk__BackingField;
                private p2.p22.Spring[] springsk__BackingField;
                private p2.p22.Body[] bodiesk__BackingField;
                private p2.p22.Solver solverk__BackingField;
                private p2.p22.Narrowphase narrowphasek__BackingField;
                private p2.p22.IslandManager islandManagerk__BackingField;
                private double[] gravityk__BackingField;
                private double frictionGravityk__BackingField;
                private bool useWorldGravityAsFrictionGravityk__BackingField;
                private bool useFrictionGravityOnZeroGravityk__BackingField;
                private bool doProfilingk__BackingField;
                private double lastStepTimek__BackingField;
                private p2.p22.Broadphase broadphasek__BackingField;
                private p2.p22.Constraint[] constraintsk__BackingField;
                private p2.p22.Material defaultMaterialk__BackingField;
                private p2.p22.ContactMaterial defaultContactMaterialk__BackingField;
                private double lastTimeStepk__BackingField;
                private bool applySpringForcesk__BackingField;
                private bool applyDampingk__BackingField;
                private bool applyGravityk__BackingField;
                private bool solveConstraintsk__BackingField;
                private p2.p22.ContactMaterial[] contactMaterialsk__BackingField;
                private double timek__BackingField;
                private bool steppingk__BackingField;
                private bool islandSplitk__BackingField;
                private bool emitImpactEventk__BackingField;
                private double sleepModek__BackingField;

                public virtual p2.p22.World.postStepEventConfig postStepEvent
                {
                    get
                    {
                        return this.postStepEventk__BackingField;
                    }
                    set
                    {
                        this.postStepEventk__BackingField = value;
                    }
                }

                public virtual p2.p22.World.addBodyEventConfig addBodyEvent
                {
                    get
                    {
                        return this.addBodyEventk__BackingField;
                    }
                    set
                    {
                        this.addBodyEventk__BackingField = value;
                    }
                }

                public virtual p2.p22.World.removeBodyEventConfig removeBodyEvent
                {
                    get
                    {
                        return this.removeBodyEventk__BackingField;
                    }
                    set
                    {
                        this.removeBodyEventk__BackingField = value;
                    }
                }

                public virtual p2.p22.World.addSpringEventConfig addSpringEvent
                {
                    get
                    {
                        return this.addSpringEventk__BackingField;
                    }
                    set
                    {
                        this.addSpringEventk__BackingField = value;
                    }
                }

                public virtual p2.p22.World.impactEventConfig impactEvent
                {
                    get
                    {
                        return this.impactEventk__BackingField;
                    }
                    set
                    {
                        this.impactEventk__BackingField = value;
                    }
                }

                public virtual p2.p22.World.postBroadphaseEventConfig postBroadphaseEvent
                {
                    get
                    {
                        return this.postBroadphaseEventk__BackingField;
                    }
                    set
                    {
                        this.postBroadphaseEventk__BackingField = value;
                    }
                }

                public virtual p2.p22.World.beginContactEventConfig beginContactEvent
                {
                    get
                    {
                        return this.beginContactEventk__BackingField;
                    }
                    set
                    {
                        this.beginContactEventk__BackingField = value;
                    }
                }

                public virtual p2.p22.World.endContactEventConfig endContactEvent
                {
                    get
                    {
                        return this.endContactEventk__BackingField;
                    }
                    set
                    {
                        this.endContactEventk__BackingField = value;
                    }
                }

                public virtual p2.p22.World.preSolveEventConfig preSolveEvent
                {
                    get
                    {
                        return this.preSolveEventk__BackingField;
                    }
                    set
                    {
                        this.preSolveEventk__BackingField = value;
                    }
                }

                public static double NO_SLEEPING
                {
                    get
                    {
                        return p2.p22.World.NO_SLEEPINGk__BackingField;
                    }
                    set
                    {
                        p2.p22.World.NO_SLEEPINGk__BackingField = value;
                    }
                }

                public static double BODY_SLEEPING
                {
                    get
                    {
                        return p2.p22.World.BODY_SLEEPINGk__BackingField;
                    }
                    set
                    {
                        p2.p22.World.BODY_SLEEPINGk__BackingField = value;
                    }
                }

                public static double ISLAND_SLEEPING
                {
                    get
                    {
                        return p2.p22.World.ISLAND_SLEEPINGk__BackingField;
                    }
                    set
                    {
                        p2.p22.World.ISLAND_SLEEPINGk__BackingField = value;
                    }
                }

                public static extern void integrateBody(p2.p22.Body body, double dy);

                public extern World();

                public extern World(p2.p22.World.Config options);

                public virtual p2.p22.Spring[] springs
                {
                    get
                    {
                        return this.springsk__BackingField;
                    }
                    set
                    {
                        this.springsk__BackingField = value;
                    }
                }

                public virtual p2.p22.Body[] bodies
                {
                    get
                    {
                        return this.bodiesk__BackingField;
                    }
                    set
                    {
                        this.bodiesk__BackingField = value;
                    }
                }

                public virtual p2.p22.Solver solver
                {
                    get
                    {
                        return this.solverk__BackingField;
                    }
                    set
                    {
                        this.solverk__BackingField = value;
                    }
                }

                public virtual p2.p22.Narrowphase narrowphase
                {
                    get
                    {
                        return this.narrowphasek__BackingField;
                    }
                    set
                    {
                        this.narrowphasek__BackingField = value;
                    }
                }

                public virtual p2.p22.IslandManager islandManager
                {
                    get
                    {
                        return this.islandManagerk__BackingField;
                    }
                    set
                    {
                        this.islandManagerk__BackingField = value;
                    }
                }

                public virtual double[] gravity
                {
                    get
                    {
                        return this.gravityk__BackingField;
                    }
                    set
                    {
                        this.gravityk__BackingField = value;
                    }
                }

                public virtual double frictionGravity
                {
                    get
                    {
                        return this.frictionGravityk__BackingField;
                    }
                    set
                    {
                        this.frictionGravityk__BackingField = value;
                    }
                }

                public virtual bool useWorldGravityAsFrictionGravity
                {
                    get
                    {
                        return this.useWorldGravityAsFrictionGravityk__BackingField;
                    }
                    set
                    {
                        this.useWorldGravityAsFrictionGravityk__BackingField = value;
                    }
                }

                public virtual bool useFrictionGravityOnZeroGravity
                {
                    get
                    {
                        return this.useFrictionGravityOnZeroGravityk__BackingField;
                    }
                    set
                    {
                        this.useFrictionGravityOnZeroGravityk__BackingField = value;
                    }
                }

                public virtual bool doProfiling
                {
                    get
                    {
                        return this.doProfilingk__BackingField;
                    }
                    set
                    {
                        this.doProfilingk__BackingField = value;
                    }
                }

                public virtual double lastStepTime
                {
                    get
                    {
                        return this.lastStepTimek__BackingField;
                    }
                    set
                    {
                        this.lastStepTimek__BackingField = value;
                    }
                }

                public virtual p2.p22.Broadphase broadphase
                {
                    get
                    {
                        return this.broadphasek__BackingField;
                    }
                    set
                    {
                        this.broadphasek__BackingField = value;
                    }
                }

                public virtual p2.p22.Constraint[] constraints
                {
                    get
                    {
                        return this.constraintsk__BackingField;
                    }
                    set
                    {
                        this.constraintsk__BackingField = value;
                    }
                }

                public virtual p2.p22.Material defaultMaterial
                {
                    get
                    {
                        return this.defaultMaterialk__BackingField;
                    }
                    set
                    {
                        this.defaultMaterialk__BackingField = value;
                    }
                }

                public virtual p2.p22.ContactMaterial defaultContactMaterial
                {
                    get
                    {
                        return this.defaultContactMaterialk__BackingField;
                    }
                    set
                    {
                        this.defaultContactMaterialk__BackingField = value;
                    }
                }

                public virtual double lastTimeStep
                {
                    get
                    {
                        return this.lastTimeStepk__BackingField;
                    }
                    set
                    {
                        this.lastTimeStepk__BackingField = value;
                    }
                }

                public virtual bool applySpringForces
                {
                    get
                    {
                        return this.applySpringForcesk__BackingField;
                    }
                    set
                    {
                        this.applySpringForcesk__BackingField = value;
                    }
                }

                public virtual bool applyDamping
                {
                    get
                    {
                        return this.applyDampingk__BackingField;
                    }
                    set
                    {
                        this.applyDampingk__BackingField = value;
                    }
                }

                public virtual bool applyGravity
                {
                    get
                    {
                        return this.applyGravityk__BackingField;
                    }
                    set
                    {
                        this.applyGravityk__BackingField = value;
                    }
                }

                public virtual bool solveConstraints
                {
                    get
                    {
                        return this.solveConstraintsk__BackingField;
                    }
                    set
                    {
                        this.solveConstraintsk__BackingField = value;
                    }
                }

                public virtual p2.p22.ContactMaterial[] contactMaterials
                {
                    get
                    {
                        return this.contactMaterialsk__BackingField;
                    }
                    set
                    {
                        this.contactMaterialsk__BackingField = value;
                    }
                }

                public virtual double time
                {
                    get
                    {
                        return this.timek__BackingField;
                    }
                    set
                    {
                        this.timek__BackingField = value;
                    }
                }

                public virtual bool stepping
                {
                    get
                    {
                        return this.steppingk__BackingField;
                    }
                    set
                    {
                        this.steppingk__BackingField = value;
                    }
                }

                public virtual bool islandSplit
                {
                    get
                    {
                        return this.islandSplitk__BackingField;
                    }
                    set
                    {
                        this.islandSplitk__BackingField = value;
                    }
                }

                public virtual bool emitImpactEvent
                {
                    get
                    {
                        return this.emitImpactEventk__BackingField;
                    }
                    set
                    {
                        this.emitImpactEventk__BackingField = value;
                    }
                }

                public virtual double sleepMode
                {
                    get
                    {
                        return this.sleepModek__BackingField;
                    }
                    set
                    {
                        this.sleepModek__BackingField = value;
                    }
                }

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
                    private string typek__BackingField;

                    public string type
                    {
                        get
                        {
                            return this.typek__BackingField;
                        }
                        set
                        {
                            this.typek__BackingField = value;
                        }
                    }

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
                    private string typek__BackingField;

                    public string type
                    {
                        get
                        {
                            return this.typek__BackingField;
                        }
                        set
                        {
                            this.typek__BackingField = value;
                        }
                    }

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
                    private string typek__BackingField;

                    public string type
                    {
                        get
                        {
                            return this.typek__BackingField;
                        }
                        set
                        {
                            this.typek__BackingField = value;
                        }
                    }

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
                    private string typek__BackingField;

                    public string type
                    {
                        get
                        {
                            return this.typek__BackingField;
                        }
                        set
                        {
                            this.typek__BackingField = value;
                        }
                    }

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
                    private string typek__BackingField;
                    private p2.p22.Body bodyAk__BackingField;
                    private p2.p22.Body bodyBk__BackingField;
                    private p2.p22.Shape shapeAk__BackingField;
                    private p2.p22.Shape shapeBk__BackingField;
                    private p2.p22.ContactEquation contactEquationk__BackingField;

                    public string type
                    {
                        get
                        {
                            return this.typek__BackingField;
                        }
                        set
                        {
                            this.typek__BackingField = value;
                        }
                    }

                    public p2.p22.Body bodyA
                    {
                        get
                        {
                            return this.bodyAk__BackingField;
                        }
                        set
                        {
                            this.bodyAk__BackingField = value;
                        }
                    }

                    public p2.p22.Body bodyB
                    {
                        get
                        {
                            return this.bodyBk__BackingField;
                        }
                        set
                        {
                            this.bodyBk__BackingField = value;
                        }
                    }

                    public p2.p22.Shape shapeA
                    {
                        get
                        {
                            return this.shapeAk__BackingField;
                        }
                        set
                        {
                            this.shapeAk__BackingField = value;
                        }
                    }

                    public p2.p22.Shape shapeB
                    {
                        get
                        {
                            return this.shapeBk__BackingField;
                        }
                        set
                        {
                            this.shapeBk__BackingField = value;
                        }
                    }

                    public p2.p22.ContactEquation contactEquation
                    {
                        get
                        {
                            return this.contactEquationk__BackingField;
                        }
                        set
                        {
                            this.contactEquationk__BackingField = value;
                        }
                    }

                    public impactEventConfig() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class postBroadphaseEventConfig : IObject
                {
                    private string typek__BackingField;
                    private p2.p22.Body[] pairsk__BackingField;

                    public string type
                    {
                        get
                        {
                            return this.typek__BackingField;
                        }
                        set
                        {
                            this.typek__BackingField = value;
                        }
                    }

                    public p2.p22.Body[] pairs
                    {
                        get
                        {
                            return this.pairsk__BackingField;
                        }
                        set
                        {
                            this.pairsk__BackingField = value;
                        }
                    }

                    public postBroadphaseEventConfig() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class beginContactEventConfig : IObject
                {
                    private string typek__BackingField;
                    private p2.p22.Shape shapeAk__BackingField;
                    private p2.p22.Shape shapeBk__BackingField;
                    private p2.p22.Body bodyAk__BackingField;
                    private p2.p22.Body bodyBk__BackingField;
                    private p2.p22.ContactEquation[] contactEquationsk__BackingField;

                    public string type
                    {
                        get
                        {
                            return this.typek__BackingField;
                        }
                        set
                        {
                            this.typek__BackingField = value;
                        }
                    }

                    public p2.p22.Shape shapeA
                    {
                        get
                        {
                            return this.shapeAk__BackingField;
                        }
                        set
                        {
                            this.shapeAk__BackingField = value;
                        }
                    }

                    public p2.p22.Shape shapeB
                    {
                        get
                        {
                            return this.shapeBk__BackingField;
                        }
                        set
                        {
                            this.shapeBk__BackingField = value;
                        }
                    }

                    public p2.p22.Body bodyA
                    {
                        get
                        {
                            return this.bodyAk__BackingField;
                        }
                        set
                        {
                            this.bodyAk__BackingField = value;
                        }
                    }

                    public p2.p22.Body bodyB
                    {
                        get
                        {
                            return this.bodyBk__BackingField;
                        }
                        set
                        {
                            this.bodyBk__BackingField = value;
                        }
                    }

                    public p2.p22.ContactEquation[] contactEquations
                    {
                        get
                        {
                            return this.contactEquationsk__BackingField;
                        }
                        set
                        {
                            this.contactEquationsk__BackingField = value;
                        }
                    }

                    public beginContactEventConfig() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class endContactEventConfig : IObject
                {
                    private string typek__BackingField;
                    private p2.p22.Shape shapeAk__BackingField;
                    private p2.p22.Shape shapeBk__BackingField;
                    private p2.p22.Body bodyAk__BackingField;
                    private p2.p22.Body bodyBk__BackingField;

                    public string type
                    {
                        get
                        {
                            return this.typek__BackingField;
                        }
                        set
                        {
                            this.typek__BackingField = value;
                        }
                    }

                    public p2.p22.Shape shapeA
                    {
                        get
                        {
                            return this.shapeAk__BackingField;
                        }
                        set
                        {
                            this.shapeAk__BackingField = value;
                        }
                    }

                    public p2.p22.Shape shapeB
                    {
                        get
                        {
                            return this.shapeBk__BackingField;
                        }
                        set
                        {
                            this.shapeBk__BackingField = value;
                        }
                    }

                    public p2.p22.Body bodyA
                    {
                        get
                        {
                            return this.bodyAk__BackingField;
                        }
                        set
                        {
                            this.bodyAk__BackingField = value;
                        }
                    }

                    public p2.p22.Body bodyB
                    {
                        get
                        {
                            return this.bodyBk__BackingField;
                        }
                        set
                        {
                            this.bodyBk__BackingField = value;
                        }
                    }

                    public endContactEventConfig() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class preSolveEventConfig : IObject
                {
                    private string typek__BackingField;
                    private p2.p22.ContactEquation[] contactEquationsk__BackingField;
                    private p2.p22.FrictionEquation[] frictionEquationsk__BackingField;

                    public string type
                    {
                        get
                        {
                            return this.typek__BackingField;
                        }
                        set
                        {
                            this.typek__BackingField = value;
                        }
                    }

                    public p2.p22.ContactEquation[] contactEquations
                    {
                        get
                        {
                            return this.contactEquationsk__BackingField;
                        }
                        set
                        {
                            this.contactEquationsk__BackingField = value;
                        }
                    }

                    public p2.p22.FrictionEquation[] frictionEquations
                    {
                        get
                        {
                            return this.frictionEquationsk__BackingField;
                        }
                        set
                        {
                            this.frictionEquationsk__BackingField = value;
                        }
                    }

                    public preSolveEventConfig() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class Config : IObject
                {
                    private p2.p22.Solver solverk__BackingField;
                    private double[] gravityk__BackingField;
                    private p2.p22.Broadphase broadphasek__BackingField;
                    private bool? islandSplitk__BackingField;
                    private bool? doProfilingk__BackingField;

                    public p2.p22.Solver solver
                    {
                        get
                        {
                            return this.solverk__BackingField;
                        }
                        set
                        {
                            this.solverk__BackingField = value;
                        }
                    }

                    public double[] gravity
                    {
                        get
                        {
                            return this.gravityk__BackingField;
                        }
                        set
                        {
                            this.gravityk__BackingField = value;
                        }
                    }

                    public p2.p22.Broadphase broadphase
                    {
                        get
                        {
                            return this.broadphasek__BackingField;
                        }
                        set
                        {
                            this.broadphasek__BackingField = value;
                        }
                    }

                    public bool? islandSplit
                    {
                        get
                        {
                            return this.islandSplitk__BackingField;
                        }
                        set
                        {
                            this.islandSplitk__BackingField = value;
                        }
                    }

                    public bool? doProfiling
                    {
                        get
                        {
                            return this.doProfilingk__BackingField;
                        }
                        set
                        {
                            this.doProfilingk__BackingField = value;
                        }
                    }

                    public Config() : base()
                    {
                        
                    }
                }

                [ObjectLiteral]
                public class setGlobalEquationParametersConfig : IObject
                {
                    private double? relaxationk__BackingField;
                    private double? stiffnessk__BackingField;

                    public double? relaxation
                    {
                        get
                        {
                            return this.relaxationk__BackingField;
                        }
                        set
                        {
                            this.relaxationk__BackingField = value;
                        }
                    }

                    public double? stiffness
                    {
                        get
                        {
                            return this.stiffnessk__BackingField;
                        }
                        set
                        {
                            this.stiffnessk__BackingField = value;
                        }
                    }

                    public setGlobalEquationParametersConfig() : base()
                    {
                        
                    }
                }
            }
        }
    }
}
