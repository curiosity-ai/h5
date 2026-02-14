using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class Sensor : dom.EventTarget
        {
            public static dom.Sensor prototype { get; set; }

            public virtual bool activated { get; }
            public virtual bool hasReading { get; }
            public virtual double? timestamp { get; }

            public virtual extern void start();
            public virtual extern void stop();

            public virtual dom.Sensor.onreadingFn onreading { get; set; }
            public virtual dom.Sensor.onerrorFn onerror { get; set; }
            public virtual dom.Sensor.onactivateFn onactivate { get; set; }

            [Generated]
            public delegate void onreadingFn(dom.Event ev);
            [Generated]
            public delegate void onerrorFn(dom.SensorErrorEvent ev);
            [Generated]
            public delegate void onactivateFn(dom.Event ev);
        }

        [CombinedClass]
        [FormerInterface]
        public class SensorErrorEvent : dom.Event
        {
            public extern SensorErrorEvent(string type, dom.SensorErrorEventInit errorInitDict);
            public static dom.SensorErrorEvent prototype { get; set; }
            public virtual dom.DOMException error { get; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class SensorErrorEventInit : dom.EventInit
        {
            public dom.DOMException error { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class SensorOptions : IObject
        {
            public double? frequency { get; set; }
        }

        [CombinedClass]
        [FormerInterface]
        public class Accelerometer : dom.Sensor
        {
            public extern Accelerometer();
            public extern Accelerometer(dom.SensorOptions options);
            public static dom.Accelerometer prototype { get; set; }

            public virtual double? x { get; }
            public virtual double? y { get; }
            public virtual double? z { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class Gyroscope : dom.Sensor
        {
            public extern Gyroscope();
            public extern Gyroscope(dom.SensorOptions options);
            public static dom.Gyroscope prototype { get; set; }

            public virtual double? x { get; }
            public virtual double? y { get; }
            public virtual double? z { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class Magnetometer : dom.Sensor
        {
            public extern Magnetometer();
            public extern Magnetometer(dom.SensorOptions options);
            public static dom.Magnetometer prototype { get; set; }

            public virtual double? x { get; }
            public virtual double? y { get; }
            public virtual double? z { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class AmbientLightSensor : dom.Sensor
        {
            public extern AmbientLightSensor();
            public extern AmbientLightSensor(dom.SensorOptions options);
            public static dom.AmbientLightSensor prototype { get; set; }

            public virtual double? illuminance { get; }
        }

        [CombinedClass]
        [FormerInterface]
        public class AbsoluteOrientationSensor : dom.Sensor
        {
            public extern AbsoluteOrientationSensor();
            public extern AbsoluteOrientationSensor(dom.SensorOptions options);
            public static dom.AbsoluteOrientationSensor prototype { get; set; }

            public virtual double[] quaternion { get; }
            public virtual extern void populateMatrix(Union<es5.Float32Array, es5.Float64Array, dom.DOMMatrix> targetMatrix);
        }

        [CombinedClass]
        [FormerInterface]
        public class RelativeOrientationSensor : dom.Sensor
        {
            public extern RelativeOrientationSensor();
            public extern RelativeOrientationSensor(dom.SensorOptions options);
            public static dom.RelativeOrientationSensor prototype { get; set; }

            public virtual double[] quaternion { get; }
            public virtual extern void populateMatrix(Union<es5.Float32Array, es5.Float64Array, dom.DOMMatrix> targetMatrix);
        }

        [Virtual]
        public abstract class SensorTypeConfig : IObject
        {
             public virtual dom.Sensor prototype { get; set; }
        }
    }
}
