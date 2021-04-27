// Decompiled with JetBrains decompiler
// Type: Retyped.howler
// Assembly: Retyped.howler, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 38EE917F-96DF-4B59-A9D5-AF9FE5C10F7F
// Assembly location: ..\retyped.howler.2.0.6733\lib\net40\Retyped.howler.dll

using System;

namespace H5.Core
{
    [Scope]
    [GlobalMethods]
    public static class howler
    {

        public static howler.HowlerGlobal Howler { get; set; }

        [Name("Howl")]
        public static howler.HowlStatic HowlType { get; set; }

        [IgnoreCast]
        [Virtual]
        [FormerInterface]
        public abstract class HowlerGlobal : IObject
        {

            public abstract void mute(bool muted);

            public abstract double volume();

            public abstract howler.HowlerGlobal volume(double volume);

            public abstract bool codecs(string ext);

            public abstract void unload();

            public virtual bool usingWebAudio { get; set; }

            public virtual bool noAudio { get; set; }

            public virtual bool mobileAutoEnable { get; set; }

            public virtual bool autoSuspend { get; set; }

            public virtual dom.AudioContext ctx { get; set; }

            public virtual dom.GainNode masterGain { get; set; }

            public abstract howler.HowlerGlobal stereo(double pan);

            public abstract Union<howler.HowlerGlobal, Void> pos(
              double x,
              double y,
              double z);

            public abstract Union<howler.HowlerGlobal, Void> orientation(
              double x,
              double y,
              double z,
              double xUp,
              double yUp,
              double zUp);

            protected HowlerGlobal() : base()
            {

            }
        }

        [IgnoreCast]
        [Virtual]
        [FormerInterface]
        public class IHowlSoundSpriteDefinition : IObject
        {
            public virtual extern Union<Sequence<double, double>, Sequence<double, double, bool>> this[
              string name]
            { get; set; }

            public IHowlSoundSpriteDefinition() : base()
            {

            }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class IHowlProperties : IObject
        {

            public Union<string, string[]> src { get; set; }

            public double? volume { get; set; }

            public bool? html5 { get; set; }

            public bool? loop { get; set; }

            public bool? preload { get; set; }

            public bool? autoplay { get; set; }

            public bool? mute { get; set; }

            public howler.IHowlSoundSpriteDefinition sprite { get; set; }

            public double? rate { get; set; }

            public double? pool { get; set; }

            public Union<string[], string> format { get; set; }

            public bool? xhrWithCredentials { get; set; }

            public Action onload { get; set; }

            public howler.IHowlProperties.onloaderrorFn onloaderror { get; set; }

            public howler.IHowlProperties.onplayFn onplay { get; set; }

            public howler.IHowlProperties.onplayerrorFn onplayerror { get; set; }

            public howler.IHowlProperties.onendFn onend { get; set; }

            public howler.IHowlProperties.onpauseFn onpause { get; set; }

            public howler.IHowlProperties.onstopFn onstop { get; set; }

            public howler.IHowlProperties.onmuteFn onmute { get; set; }

            public howler.IHowlProperties.onvolumeFn onvolume { get; set; }

            public howler.IHowlProperties.onrateFn onrate { get; set; }

            public howler.IHowlProperties.onseekFn onseek { get; set; }

            public howler.IHowlProperties.onfadeFn onfade { get; set; }

            public IHowlProperties() : base()
            {

            }

            [Generated]
            public delegate void onloaderrorFn(double soundId, object error);

            [Generated]
            public delegate void onplayFn(double soundId);

            [Generated]
            public delegate void onplayerrorFn(double soundId, object error);

            [Generated]
            public delegate void onendFn(double soundId);

            [Generated]
            public delegate void onpauseFn(double soundId);

            [Generated]
            public delegate void onstopFn(double soundId);

            [Generated]
            public delegate void onmuteFn(double soundId);

            [Generated]
            public delegate void onvolumeFn(double soundId);

            [Generated]
            public delegate void onrateFn(double soundId);

            [Generated]
            public delegate void onseekFn(double soundId);

            [Generated]
            public delegate void onfadeFn(double soundId);
        }

        [CombinedClass]
        [StaticInterface("HowlStatic")]
        [FormerInterface]
        public class Howl : IObject
        {
            public extern Howl(howler.IHowlProperties properties);

            public virtual extern double play();

            public virtual extern double play(Union<string, double> spriteOrId);

            public virtual extern double play(string spriteOrId);

            public virtual extern double play(double spriteOrId);

            public virtual extern howler.Howl pause();

            public virtual extern howler.Howl pause(double id);

            public virtual extern howler.Howl stop();

            public virtual extern howler.Howl stop(double id);

            public virtual extern bool mute();

            public virtual extern howler.Howl mute(bool muted);

            public virtual extern howler.Howl mute(bool muted, double id);

            public virtual extern double volume();

            public virtual extern Union<howler.Howl, double> volume(double idOrSetVolume);

            public virtual extern howler.Howl volume(double volume, double id);

            public virtual extern howler.Howl fade(double from, double to, double duration);

            public virtual extern howler.Howl fade(
              double from,
              double to,
              double duration,
              double id);

            public virtual extern double rate();

            public virtual extern Union<howler.Howl, double> rate(double idOrSetRate);

            public virtual extern howler.Howl rate(double rate, double id);

            public virtual extern Union<howler.Howl, double> seek();

            public virtual extern Union<howler.Howl, double> seek(double seek);

            public virtual extern Union<howler.Howl, double> seek(double seek, double id);

            public virtual extern howler.Howl loop();

            public virtual extern howler.Howl loop(bool loop);

            public virtual extern howler.Howl loop(bool loop, double id);

            public virtual extern bool playing();

            public virtual extern bool playing(double id);

            public virtual extern double duration();

            public virtual extern double duration(double id);

            public virtual extern howler.Howl on(howler.Literals.Types.load @event, Action callback);

            public virtual extern howler.Howl on(
              howler.Literals.Types.load @event,
              Action callback,
              double id);

            public virtual extern howler.Howl on(
              howler.Literals.Types.loaderror @event,
              howler.Howl.onFn callback);

            public virtual extern howler.Howl on(
              howler.Literals.Types.loaderror @event,
              howler.Howl.onFn callback,
              double id);

            public virtual extern howler.Howl on(
              howler.Literals.Types.play @event,
              howler.Howl.onFn2 callback);

            public virtual extern howler.Howl on(
              howler.Literals.Types.play @event,
              howler.Howl.onFn2 callback,
              double id);

            public virtual extern howler.Howl on(
              howler.Literals.Types.playerror @event,
              howler.Howl.onFn callback);

            public virtual extern howler.Howl on(
              howler.Literals.Types.playerror @event,
              howler.Howl.onFn callback,
              double id);

            public virtual extern howler.Howl on(
              howler.Literals.Types.end @event,
              howler.Howl.onFn2 callback);

            public virtual extern howler.Howl on(
              howler.Literals.Types.end @event,
              howler.Howl.onFn2 callback,
              double id);

            public virtual extern howler.Howl on(
              howler.Literals.Types.pause @event,
              howler.Howl.onFn2 callback);

            public virtual extern howler.Howl on(
              howler.Literals.Types.pause @event,
              howler.Howl.onFn2 callback,
              double id);

            public virtual extern howler.Howl on(
              howler.Literals.Types.stop @event,
              howler.Howl.onFn2 callback);

            public virtual extern howler.Howl on(
              howler.Literals.Types.stop @event,
              howler.Howl.onFn2 callback,
              double id);

            public virtual extern howler.Howl on(
              howler.Literals.Types.mute @event,
              howler.Howl.onFn2 callback);

            public virtual extern howler.Howl on(
              howler.Literals.Types.mute @event,
              howler.Howl.onFn2 callback,
              double id);

            public virtual extern howler.Howl on(
              howler.Literals.Types.volume @event,
              howler.Howl.onFn2 callback);

            public virtual extern howler.Howl on(
              howler.Literals.Types.volume @event,
              howler.Howl.onFn2 callback,
              double id);

            public virtual extern howler.Howl on(
              howler.Literals.Types.rate @event,
              howler.Howl.onFn2 callback);

            public virtual extern howler.Howl on(
              howler.Literals.Types.rate @event,
              howler.Howl.onFn2 callback,
              double id);

            public virtual extern howler.Howl on(
              howler.Literals.Types.seek @event,
              howler.Howl.onFn2 callback);

            public virtual extern howler.Howl on(
              howler.Literals.Types.seek @event,
              howler.Howl.onFn2 callback,
              double id);

            public virtual extern howler.Howl on(
              howler.Literals.Types.fade @event,
              howler.Howl.onFn2 callback);

            public virtual extern howler.Howl on(
              howler.Literals.Types.fade @event,
              howler.Howl.onFn2 callback,
              double id);

            public virtual extern howler.Howl on(string @event, es5.Function callback);

            public virtual extern howler.Howl on(string @event, Action callback);

            public virtual extern howler.Howl on(string @event, Func<object> callback);

            public virtual extern howler.Howl on(string @event, es5.Function callback, double id);

            public virtual extern howler.Howl on(string @event, Action callback, double id);

            public virtual extern howler.Howl on(string @event, Func<object> callback, double id);

            public virtual extern howler.Howl once(
              howler.Literals.Types.load @event,
              Action callback);

            public virtual extern howler.Howl once(
              howler.Literals.Types.load @event,
              Action callback,
              double id);

            public virtual extern howler.Howl once(
              howler.Literals.Types.loaderror @event,
              howler.Howl.onceFn callback);

            public virtual extern howler.Howl once(
              howler.Literals.Types.loaderror @event,
              howler.Howl.onceFn callback,
              double id);

            public virtual extern howler.Howl once(
              howler.Literals.Types.play @event,
              howler.Howl.onceFn2 callback);

            public virtual extern howler.Howl once(
              howler.Literals.Types.play @event,
              howler.Howl.onceFn2 callback,
              double id);

            public virtual extern howler.Howl once(
              howler.Literals.Types.playerror @event,
              howler.Howl.onceFn callback);

            public virtual extern howler.Howl once(
              howler.Literals.Types.playerror @event,
              howler.Howl.onceFn callback,
              double id);

            public virtual extern howler.Howl once(
              howler.Literals.Types.end @event,
              howler.Howl.onceFn2 callback);

            public virtual extern howler.Howl once(
              howler.Literals.Types.end @event,
              howler.Howl.onceFn2 callback,
              double id);

            public virtual extern howler.Howl once(
              howler.Literals.Types.pause @event,
              howler.Howl.onceFn2 callback);

            public virtual extern howler.Howl once(
              howler.Literals.Types.pause @event,
              howler.Howl.onceFn2 callback,
              double id);

            public virtual extern howler.Howl once(
              howler.Literals.Types.stop @event,
              howler.Howl.onceFn2 callback);

            public virtual extern howler.Howl once(
              howler.Literals.Types.stop @event,
              howler.Howl.onceFn2 callback,
              double id);

            public virtual extern howler.Howl once(
              howler.Literals.Types.mute @event,
              howler.Howl.onceFn2 callback);

            public virtual extern howler.Howl once(
              howler.Literals.Types.mute @event,
              howler.Howl.onceFn2 callback,
              double id);

            public virtual extern howler.Howl once(
              howler.Literals.Types.volume @event,
              howler.Howl.onceFn2 callback);

            public virtual extern howler.Howl once(
              howler.Literals.Types.volume @event,
              howler.Howl.onceFn2 callback,
              double id);

            public virtual extern howler.Howl once(
              howler.Literals.Types.rate @event,
              howler.Howl.onceFn2 callback);

            public virtual extern howler.Howl once(
              howler.Literals.Types.rate @event,
              howler.Howl.onceFn2 callback,
              double id);

            public virtual extern howler.Howl once(
              howler.Literals.Types.seek @event,
              howler.Howl.onceFn2 callback);

            public virtual extern howler.Howl once(
              howler.Literals.Types.seek @event,
              howler.Howl.onceFn2 callback,
              double id);

            public virtual extern howler.Howl once(
              howler.Literals.Types.fade @event,
              howler.Howl.onceFn2 callback);

            public virtual extern howler.Howl once(
              howler.Literals.Types.fade @event,
              howler.Howl.onceFn2 callback,
              double id);

            public virtual extern howler.Howl once(string @event, es5.Function callback);

            public virtual extern howler.Howl once(string @event, Action callback);

            public virtual extern howler.Howl once(string @event, Func<object> callback);

            public virtual extern howler.Howl once(string @event, es5.Function callback, double id);

            public virtual extern howler.Howl once(string @event, Action callback, double id);

            public virtual extern howler.Howl once(string @event, Func<object> callback, double id);

            public virtual extern howler.Howl off(string @event);

            public virtual extern howler.Howl off(string @event, es5.Function callback);

            public virtual extern howler.Howl off(string @event, Action callback);

            public virtual extern howler.Howl off(string @event, Func<object> callback);

            public virtual extern howler.Howl off(string @event, es5.Function callback, double id);

            public virtual extern howler.Howl off(string @event, Action callback, double id);

            public virtual extern howler.Howl off(string @event, Func<object> callback, double id);

            public virtual extern howler.Literals.Options.state state();

            public virtual extern void load();

            public virtual extern void unload();

            public virtual extern Union<howler.Howl, Void> stereo(double pan);

            public virtual extern Union<howler.Howl, Void> stereo(double pan, double id);

            public virtual extern Union<howler.Howl, Void> pos(
              double x,
              double y,
              double z);

            public virtual extern Union<howler.Howl, Void> pos(
              double x,
              double y,
              double z,
              double id);

            public virtual extern Union<howler.Howl, Void> orientation(
              double x,
              double y,
              double z,
              double xUp,
              double yUp,
              double zUp);

            public virtual extern howler.Howl pannerAttr(howler.Howl.pannerAttrConfig o);

            public virtual extern howler.Howl pannerAttr(howler.Howl.pannerAttrConfig o, double id);

            [Generated]
            public delegate void onFn(double soundId, object error);

            [Generated]
            public delegate void onFn2(double soundId);

            [Generated]
            public delegate void onceFn(double soundId, object error);

            [Generated]
            public delegate void onceFn2(double soundId);

            [ObjectLiteral]
            public class pannerAttrConfig : IObject
            {

                public double? coneInnerAngle { get; set; }

                public double? coneOuterAngle { get; set; }

                public double? coneOuterGain { get; set; }

                public howler.Literals.Options.distanceModel distanceModel { get; set; }

                public double maxDistance { get; set; }

                public howler.Literals.Options.panningModel panningModel { get; set; }

                public double refDistance { get; set; }

                public double rolloffFactor { get; set; }

                public pannerAttrConfig() : base()
                {

                }
            }
        }

        [IgnoreCast]
        public interface HowlStatic : IObject
        {
            [Template("new {this}({0})")]
            howler.Howl New(howler.IHowlProperties properties);
        }

        [Scope]
        [GlobalMethods]
        [Module]
        public static class howler2
        {

            public static howler.HowlerGlobal Howler { get; set; }

            public static howler.HowlStatic Howl { get; set; }
        }

        public static class Literals
        {
            [Template("<self>\"load\"")]
            public static readonly howler.Literals.Types.load load;
            [Template("<self>\"loaderror\"")]
            public static readonly howler.Literals.Types.loaderror loaderror;
            [Template("<self>\"play\"")]
            public static readonly howler.Literals.Types.play play;
            [Template("<self>\"playerror\"")]
            public static readonly howler.Literals.Types.playerror playerror;
            [Template("<self>\"end\"")]
            public static readonly howler.Literals.Types.end end;
            [Template("<self>\"pause\"")]
            public static readonly howler.Literals.Types.pause pause;
            [Template("<self>\"stop\"")]
            public static readonly howler.Literals.Types.stop stop;
            [Template("<self>\"mute\"")]
            public static readonly howler.Literals.Types.mute mute;
            [Template("<self>\"volume\"")]
            public static readonly howler.Literals.Types.volume volume;
            [Template("<self>\"rate\"")]
            public static readonly howler.Literals.Types.rate rate;
            [Template("<self>\"seek\"")]
            public static readonly howler.Literals.Types.seek seek;
            [Template("<self>\"fade\"")]
            public static readonly howler.Literals.Types.fade fade;
            [Template("<self>\"unloaded\"")]
            public static readonly howler.Literals.Types.unloaded unloaded;
            [Template("<self>\"loading\"")]
            public static readonly howler.Literals.Types.loading loading;
            [Template("<self>\"loaded\"")]
            public static readonly howler.Literals.Types.loaded loaded;
            [Template("<self>\"inverse\"")]
            public static readonly howler.Literals.Types.inverse inverse;
            [Template("<self>\"linear\"")]
            public static readonly howler.Literals.Types.linear linear;
            [Template("<self>\"HRTF\"")]
            public static readonly howler.Literals.Types.HRTF HRTF;
            [Template("<self>\"equalpower\"")]
            public static readonly howler.Literals.Types.equalpower equalpower;

            public static class Types
            {
                [Name("System.String")]
                public class load : LiteralType<string>
                {
                    private extern load();
                }

                [Name("System.String")]
                public class loaderror : LiteralType<string>
                {
                    private extern loaderror();
                }

                [Name("System.String")]
                public class play : LiteralType<string>
                {
                    private extern play();
                }

                [Name("System.String")]
                public class playerror : LiteralType<string>
                {
                    private extern playerror();
                }

                [Name("System.String")]
                public class end : LiteralType<string>
                {
                    private extern end();
                }

                [Name("System.String")]
                public class pause : LiteralType<string>
                {
                    private extern pause();
                }

                [Name("System.String")]
                public class stop : LiteralType<string>
                {
                    private extern stop();
                }

                [Name("System.String")]
                public class mute : LiteralType<string>
                {
                    private extern mute();
                }

                [Name("System.String")]
                public class volume : LiteralType<string>
                {
                    private extern volume();
                }

                [Name("System.String")]
                public class rate : LiteralType<string>
                {
                    private extern rate();
                }

                [Name("System.String")]
                public class seek : LiteralType<string>
                {
                    private extern seek();
                }

                [Name("System.String")]
                public class fade : LiteralType<string>
                {
                    private extern fade();
                }

                [Name("System.String")]
                public class unloaded : LiteralType<string>
                {
                    private extern unloaded();
                }

                [Name("System.String")]
                public class loading : LiteralType<string>
                {
                    private extern loading();
                }

                [Name("System.String")]
                public class loaded : LiteralType<string>
                {
                    private extern loaded();
                }

                [Name("System.String")]
                public class inverse : LiteralType<string>
                {
                    private extern inverse();
                }

                [Name("System.String")]
                public class linear : LiteralType<string>
                {
                    private extern linear();
                }

                [Name("System.String")]
                public class HRTF : LiteralType<string>
                {
                    private extern HRTF();
                }

                [Name("System.String")]
                public class equalpower : LiteralType<string>
                {
                    private extern equalpower();
                }
            }

            public static class Options
            {
                [Name("System.String")]
                public class state : LiteralType<string>
                {
                    [Template("<self>\"unloaded\"")]
                    public static readonly howler.Literals.Types.unloaded unloaded;
                    [Template("<self>\"loading\"")]
                    public static readonly howler.Literals.Types.loading loading;
                    [Template("<self>\"loaded\"")]
                    public static readonly howler.Literals.Types.loaded loaded;

                    private extern state();

                    public static extern implicit operator howler.Literals.Options.state(
                      howler.Literals.Types.unloaded value);

                    public static extern implicit operator howler.Literals.Options.state(
                      howler.Literals.Types.loading value);

                    public static extern implicit operator howler.Literals.Options.state(
                      howler.Literals.Types.loaded value);
                }

                [Name("System.String")]
                public class distanceModel : LiteralType<string>
                {
                    [Template("<self>\"inverse\"")]
                    public static readonly howler.Literals.Types.inverse inverse;
                    [Template("<self>\"linear\"")]
                    public static readonly howler.Literals.Types.linear linear;

                    private extern distanceModel();

                    public static extern implicit operator howler.Literals.Options.distanceModel(
                      howler.Literals.Types.inverse value);

                    public static extern implicit operator howler.Literals.Options.distanceModel(
                      howler.Literals.Types.linear value);
                }

                [Name("System.String")]
                public class panningModel : LiteralType<string>
                {
                    [Template("<self>\"HRTF\"")]
                    public static readonly howler.Literals.Types.HRTF HRTF;
                    [Template("<self>\"equalpower\"")]
                    public static readonly howler.Literals.Types.equalpower equalpower;

                    private extern panningModel();

                    public static extern implicit operator howler.Literals.Options.panningModel(
                      howler.Literals.Types.HRTF value);

                    public static extern implicit operator howler.Literals.Options.panningModel(
                      howler.Literals.Types.equalpower value);
                }
            }
        }
    }
}
