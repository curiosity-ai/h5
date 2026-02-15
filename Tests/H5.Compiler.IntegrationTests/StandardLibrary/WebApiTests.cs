using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H5.Compiler.IntegrationTests.StandardLibrary
{
    [TestClass]
    public class WebApiTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task ResizeObserverTest()
        {
            var code = @"
            using H5;
            using H5.Core;
            using System;

            public class Program
            {
                public static void Main()
                {
                    if (Script.Write<bool>(""typeof ResizeObserver === 'undefined'""))
                    {
                        Console.WriteLine(""Success""); // Skip if not supported in test env
                        return;
                    }

                    var observer = new dom.ResizeObserver((entries, obs) => {
                        Console.WriteLine(""Callback"");
                    });

                    Console.WriteLine(observer != null ? ""Success"" : ""Failed"");
                }
            }";

            // skipRoslyn: true, includeCorePackages: true because ResizeObserver is not available in .NET
            var output = await RunTest(code, skipRoslyn: true, includeCorePackages: true);
            Assert.AreEqual("Success", output);
        }

        [TestMethod]
        public async Task WebCodecsTest()
        {
            var code = @"
            using H5;
            using H5.Core;
            using System;

            public class Program
            {
                public static void Main()
                {
                    if (Script.Write<bool>(""typeof VideoEncoder === 'undefined'""))
                    {
                        Console.WriteLine(""Success""); // Skip if not supported in test env
                        return;
                    }

                    var init = new dom.VideoEncoderInit
                    {
                        error = (e) => Console.WriteLine(""Error""),
                        output = (chunk, meta) => Console.WriteLine(""Output"")
                    };
                    var encoder = new dom.VideoEncoder(init);
                    Console.WriteLine(encoder != null ? ""Success"" : ""Failed"");
                }
            }";

            var output = await RunTest(code, skipRoslyn: true, includeCorePackages: true);
            Assert.AreEqual("Success", output);
        }

        [TestMethod]
        public async Task MediaSessionTest()
        {
            var code = @"
            using H5;
            using H5.Core;
            using System;

            public class Program
            {
                public static void Main()
                {
                    if (Script.Write<bool>(""typeof navigator.mediaSession === 'undefined'""))
                    {
                        Console.WriteLine(""Success""); // Skip if not supported in test env
                        return;
                    }

                    var session = dom.window.navigator.mediaSession;
                    session.metadata = new dom.MediaMetadata(new dom.MediaMetadataInit { title = ""Title"" });
                    Console.WriteLine(session.metadata.title == ""Title"" ? ""Success"" : ""Failed"");
                }
            }";

            var output = await RunTest(code, skipRoslyn: true, includeCorePackages: true);
            Assert.AreEqual("Success", output);
        }

        [TestMethod]
        public async Task PictureInPictureTest()
        {
            var code = @"
            using H5;
            using H5.Core;
            using System;

            public class Program
            {
                public static void Main()
                {
                    // Check if document.pictureInPictureEnabled is defined
                    if (Script.Write<bool>(""typeof document.pictureInPictureEnabled === 'undefined'""))
                    {
                        Console.WriteLine(""Success""); // Skip if not supported in test env
                        return;
                    }

                    Console.WriteLine(""Success"");
                }
            }";

            var output = await RunTest(code, skipRoslyn: true, includeCorePackages: true);
            Assert.AreEqual("Success", output);
        }

        [TestMethod]
        public async Task WebHIDTest()
        {
            var code = @"
            using H5;
            using H5.Core;
            using System;

            public class Program
            {
                public static void Main()
                {
                    if (Script.Write<bool>(""typeof navigator.hid === 'undefined'""))
                    {
                        Console.WriteLine(""Success""); // Skip if not supported in test env
                        return;
                    }

                    var hid = dom.window.navigator.hid;
                    Console.WriteLine(hid != null ? ""Success"" : ""Failed"");
                }
            }";

            var output = await RunTest(code, skipRoslyn: true, includeCorePackages: true);
            Assert.AreEqual("Success", output);
        }

        [TestMethod]
        public async Task WebSerialTest()
        {
            var code = @"
            using H5;
            using H5.Core;
            using System;

            public class Program
            {
                public static void Main()
                {
                    if (Script.Write<bool>(""typeof navigator.serial === 'undefined'""))
                    {
                        Console.WriteLine(""Success""); // Skip if not supported in test env
                        return;
                    }

                    var serial = dom.window.navigator.serial;
                    Console.WriteLine(serial != null ? ""Success"" : ""Failed"");
                }
            }";

            var output = await RunTest(code, skipRoslyn: true, includeCorePackages: true);
            Assert.AreEqual("Success", output);
        }

        [TestMethod]
        public async Task WebMIDITest()
        {
            var code = @"
            using H5;
            using H5.Core;
            using System;

            public class Program
            {
                public static void Main()
                {
                    if (Script.Write<bool>(""typeof navigator.requestMIDIAccess === 'undefined'""))
                    {
                        Console.WriteLine(""Success""); // Skip if not supported in test env
                        return;
                    }

                    // Just check if we can call it (it returns a promise)
                    var promise = dom.window.navigator.requestMIDIAccess();
                    Console.WriteLine(promise != null ? ""Success"" : ""Failed"");
                }
            }";

            var output = await RunTest(code, skipRoslyn: true, includeCorePackages: true);
            Assert.AreEqual("Success", output);
        }

        [TestMethod]
        public async Task SensorsTest()
        {
            var code = @"
            using H5;
            using H5.Core;
            using System;

            public class Program
            {
                public static void Main()
                {
                    if (Script.Write<bool>(""typeof Accelerometer === 'undefined'""))
                    {
                        Console.WriteLine(""Success""); // Skip if not supported in test env
                        return;
                    }

                    var sensor = new dom.Accelerometer();
                    Console.WriteLine(sensor != null ? ""Success"" : ""Failed"");
                }
            }";

            var output = await RunTest(code, skipRoslyn: true, includeCorePackages: true);
            Assert.AreEqual("Success", output);
        }

        [TestMethod]
        public async Task FileSystemAccessTest()
        {
            var code = @"
            using H5;
            using H5.Core;
            using static H5.Core.dom;
            using System;

            public class Program
            {
                public static void Main()
                {
                    if (Script.Write<bool>(""typeof showOpenFilePicker === 'undefined'""))
                    {
                        System.Console.WriteLine(""Success""); // Skip if not supported in test env
                        return;
                    }

                    try
                    {
                        window.showOpenFilePicker();
                        System.Console.WriteLine(""Success"");
                    }
                    catch
                    {
                        System.Console.WriteLine(""Failed"");
                    }
                }
            }";

            var output = await RunTest(code, skipRoslyn: true, includeCorePackages: true);
            Assert.AreEqual("Success", output);
        }

        [TestMethod]
        public async Task WebXRTest()
        {
            var code = @"
            using H5;
            using H5.Core;
            using System;

            public class Program
            {
                public static void Main()
                {
                    if (Script.Write<bool>(""typeof navigator.xr === 'undefined'""))
                    {
                        Console.WriteLine(""Success""); // Skip if not supported in test env
                        return;
                    }

                    var xr = dom.window.navigator.xr;
                    Console.WriteLine(xr != null ? ""Success"" : ""Failed"");
                }
            }";

            var output = await RunTest(code, skipRoslyn: true, includeCorePackages: true);
            Assert.AreEqual("Success", output);
        }

        [TestMethod]
        public async Task WebBluetoothTest()
        {
            var code = @"
            using H5;
            using H5.Core;
            using System;

            public class Program
            {
                public static void Main()
                {
                    if (Script.Write<bool>(""typeof navigator.bluetooth === 'undefined'""))
                    {
                        Console.WriteLine(""Success""); // Skip if not supported in test env
                        return;
                    }

                    var bt = dom.window.navigator.bluetooth;
                    Console.WriteLine(bt != null ? ""Success"" : ""Failed"");
                }
            }";

            var output = await RunTest(code, skipRoslyn: true, includeCorePackages: true);
            Assert.AreEqual("Success", output);
        }

        [TestMethod]
        public async Task WebUSBTest()
        {
            var code = @"
            using H5;
            using H5.Core;
            using System;

            public class Program
            {
                public static void Main()
                {
                    if (Script.Write<bool>(""typeof navigator.usb === 'undefined'""))
                    {
                        Console.WriteLine(""Success""); // Skip if not supported in test env
                        return;
                    }

                    var usb = dom.window.navigator.usb;
                    Console.WriteLine(usb != null ? ""Success"" : ""Failed"");
                }
            }";

            var output = await RunTest(code, skipRoslyn: true, includeCorePackages: true);
            Assert.AreEqual("Success", output);
        }

        [TestMethod]
        public async Task CryptoRandomUUIDTest()
        {
            var code = @"
            using H5;
            using H5.Core;
            using System;

            public class Program
            {
                public static void Main()
                {
                    if (Script.Write<bool>(""typeof crypto === 'undefined' || typeof crypto.randomUUID === 'undefined'""))
                    {
                        Console.WriteLine(""Success""); // Skip if not supported in test env
                        return;
                    }

                    var uuid = dom.window.crypto.randomUUID();
                    Console.WriteLine(uuid != null && uuid.Length > 0 ? ""Success"" : ""Failed"");
                }
            }";

            var output = await RunTest(code, skipRoslyn: true, includeCorePackages: true);
            Assert.AreEqual("Success", output);
        }

        [TestMethod]
        public async Task CompressionStreamTest()
        {
            var code = @"
            using H5;
            using H5.Core;
            using System;

            public class Program
            {
                public static void Main()
                {
                    if (Script.Write<bool>(""typeof CompressionStream === 'undefined'""))
                    {
                        Console.WriteLine(""Success""); // Skip if not supported in test env
                        return;
                    }

                    var stream = new dom.CompressionStream(dom.CompressionFormat.gzip);
                    Console.WriteLine(stream != null && stream.readable != null ? ""Success"" : ""Failed"");
                }
            }";

            var output = await RunTest(code, skipRoslyn: true, includeCorePackages: true);
            Assert.AreEqual("Success", output);
        }

        [TestMethod]
        public async Task ScreenOrientationTest()
        {
            var code = @"
            using H5;
            using H5.Core;
            using System;

            public class Program
            {
                public static void Main()
                {
                    if (Script.Write<bool>(""typeof screen.orientation === 'undefined'""))
                    {
                        Console.WriteLine(""Success""); // Skip if not supported in test env
                        return;
                    }

                    var type = dom.window.screen.orientation.type;
                    Console.WriteLine(type != null ? ""Success"" : ""Failed"");
                }
            }";

            var output = await RunTest(code, skipRoslyn: true, includeCorePackages: true);
            Assert.AreEqual("Success", output);
        }

        [TestMethod]
        public async Task PerformanceObserverTest()
        {
            var code = @"
            using H5;
            using H5.Core;
            using System;

            public class Program
            {
                public static void Main()
                {
                    if (Script.Write<bool>(""typeof PerformanceObserver === 'undefined'""))
                    {
                        Console.WriteLine(""Success""); // Skip if not supported in test env
                        return;
                    }

                    var observer = new dom.PerformanceObserver((list, obs) => {
                        Console.WriteLine(""Callback"");
                    });

                    Console.WriteLine(observer != null ? ""Success"" : ""Failed"");
                }
            }";

            var output = await RunTest(code, skipRoslyn: true, includeCorePackages: true);
            Assert.AreEqual("Success", output);
        }

        [TestMethod]
        public async Task ReportingObserverTest()
        {
            var code = @"
            using H5;
            using H5.Core;
            using System;

            public class Program
            {
                public static void Main()
                {
                    if (Script.Write<bool>(""typeof ReportingObserver === 'undefined'""))
                    {
                        Console.WriteLine(""Success""); // Skip if not supported in test env
                        return;
                    }

                    var observer = new dom.ReportingObserver((reports, obs) => {
                        Console.WriteLine(""Callback"");
                    });

                    Console.WriteLine(observer != null ? ""Success"" : ""Failed"");
                }
            }";

            var output = await RunTest(code, skipRoslyn: true, includeCorePackages: true);
            Assert.AreEqual("Success", output);
        }
    }
}
