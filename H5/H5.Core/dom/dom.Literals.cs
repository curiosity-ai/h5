// Decompiled with JetBrains decompiler
// Type: H5.dom
// Assembly: H5.dom, Version=2.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 57CCBF73-D494-47BA-ACF8-95E65E795865
// Assembly location: C:\work\curiosity\tesserae\Tesserae\bin\Debug\net461\H5.dom.dll

using H5;
using H5.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace H5.Core
{
    public static partial class dom
    {
        public static class Literals
        {
            [Template("<self>\"drag\"")]
            public static readonly dom.Literals.Types.drag drag;
            [Template("<self>\"dragend\"")]
            public static readonly dom.Literals.Types.dragend dragend;
            [Template("<self>\"dragenter\"")]
            public static readonly dom.Literals.Types.dragenter dragenter;
            [Template("<self>\"dragexit\"")]
            public static readonly dom.Literals.Types.dragexit dragexit;
            [Template("<self>\"dragleave\"")]
            public static readonly dom.Literals.Types.dragleave dragleave;
            [Template("<self>\"dragover\"")]
            public static readonly dom.Literals.Types.dragover dragover;
            [Template("<self>\"dragstart\"")]
            public static readonly dom.Literals.Types.dragstart dragstart;
            [Template("<self>\"drop\"")]
            public static readonly dom.Literals.Types.drop drop;
            [Template("<self>\"auto\"")]
            public static readonly dom.Literals.Types.auto auto;
            [Template("<self>\"instant\"")]
            public static readonly dom.Literals.Types.instant instant;
            [Template("<self>\"smooth\"")]
            public static readonly dom.Literals.Types.smooth smooth;
            [Template("<self>\"start\"")]
            public static readonly dom.Literals.Types.start start;
            [Template("<self>\"center\"")]
            public static readonly dom.Literals.Types.center center;
            [Template("<self>\"end\"")]
            public static readonly dom.Literals.Types.end end;
            [Template("<self>\"nearest\"")]
            public static readonly dom.Literals.Types.nearest nearest;
            [Template("<self>\"manual\"")]
            public static readonly dom.Literals.Types.manual manual;
            [Template("<self>\"beforebegin\"")]
            public static readonly dom.Literals.Types.beforebegin beforebegin;
            [Template("<self>\"afterbegin\"")]
            public static readonly dom.Literals.Types.afterbegin afterbegin;
            [Template("<self>\"beforeend\"")]
            public static readonly dom.Literals.Types.beforeend beforeend;
            [Template("<self>\"afterend\"")]
            public static readonly dom.Literals.Types.afterend afterend;
            [Template("<self>\"any\"")]
            public static readonly dom.Literals.Types.any any;
            [Template("<self>\"natural\"")]
            public static readonly dom.Literals.Types.natural natural;
            [Template("<self>\"portrait\"")]
            public static readonly dom.Literals.Types.portrait portrait;
            [Template("<self>\"landscape\"")]
            public static readonly dom.Literals.Types.landscape landscape;
            [Template("<self>\"portrait-primary\"")]
            public static readonly dom.Literals.Types.portrait_primary portrait_primary;
            [Template("<self>\"portrait-secondary\"")]
            public static readonly dom.Literals.Types.portrait_secondary portrait_secondary;
            [Template("<self>\"landscape-primary\"")]
            public static readonly dom.Literals.Types.landscape_primary landscape_primary;
            [Template("<self>\"landscape-secondary\"")]
            public static readonly dom.Literals.Types.landscape_secondary landscape_secondary;
            [Template("<self>\"attributes\"")]
            public static readonly dom.Literals.Types.attributes attributes;
            [Template("<self>\"characterData\"")]
            public static readonly dom.Literals.Types.characterData characterData;
            [Template("<self>\"childList\"")]
            public static readonly dom.Literals.Types.childList childList;
            [Template("<self>\"window\"")]
            public static readonly dom.Literals.Types.window window;
            [Template("<self>\"worker\"")]
            public static readonly dom.Literals.Types.worker worker;
            [Template("<self>\"sharedworker\"")]
            public static readonly dom.Literals.Types.sharedworker sharedworker;
            [Template("<self>\"all\"")]
            public static readonly dom.Literals.Types.all all;
            [Template("<self>\"segments\"")]
            public static readonly dom.Literals.Types.segments segments;
            [Template("<self>\"sequence\"")]
            public static readonly dom.Literals.Types.sequence sequence;
            [Template("<self>\"balanced\"")]
            public static readonly dom.Literals.Types.balanced balanced;
            [Template("<self>\"interactive\"")]
            public static readonly dom.Literals.Types.interactive interactive;
            [Template("<self>\"playback\"")]
            public static readonly dom.Literals.Types.playback playback;
            [Template("<self>\"suspended\"")]
            public static readonly dom.Literals.Types.suspended suspended;
            [Template("<self>\"running\"")]
            public static readonly dom.Literals.Types.running running;
            [Template("<self>\"closed\"")]
            public static readonly dom.Literals.Types.closed closed;
            [Template("<self>\"blob\"")]
            public static readonly dom.Literals.Types.blob blob;
            [Template("<self>\"arraybuffer\"")]
            public static readonly dom.Literals.Types.arraybuffer arraybuffer;
            [Template("<self>\"lowpass\"")]
            public static readonly dom.Literals.Types.lowpass lowpass;
            [Template("<self>\"highpass\"")]
            public static readonly dom.Literals.Types.highpass highpass;
            [Template("<self>\"bandpass\"")]
            public static readonly dom.Literals.Types.bandpass bandpass;
            [Template("<self>\"lowshelf\"")]
            public static readonly dom.Literals.Types.lowshelf lowshelf;
            [Template("<self>\"highshelf\"")]
            public static readonly dom.Literals.Types.highshelf highshelf;
            [Template("<self>\"peaking\"")]
            public static readonly dom.Literals.Types.peaking peaking;
            [Template("<self>\"notch\"")]
            public static readonly dom.Literals.Types.notch notch;
            [Template("<self>\"allpass\"")]
            public static readonly dom.Literals.Types.allpass allpass;
            [Template("<self>\"\"")]
            public static readonly dom.Literals.Types._ _;
            [Template("<self>\"maybe\"")]
            public static readonly dom.Literals.Types.maybe maybe;
            [Template("<self>\"probably\"")]
            public static readonly dom.Literals.Types.probably probably;
            [Template("<self>\"nonzero\"")]
            public static readonly dom.Literals.Types.nonzero nonzero;
            [Template("<self>\"evenodd\"")]
            public static readonly dom.Literals.Types.evenodd evenodd;
            [Template("<self>\"max\"")]
            public static readonly dom.Literals.Types.max max;
            [Template("<self>\"clamped-max\"")]
            public static readonly dom.Literals.Types.clamped_max clamped_max;
            [Template("<self>\"explicit\"")]
            public static readonly dom.Literals.Types.@explicit @explicit;
            [Template("<self>\"speakers\"")]
            public static readonly dom.Literals.Types.speakers speakers;
            [Template("<self>\"discrete\"")]
            public static readonly dom.Literals.Types.discrete discrete;
            [Template("<self>\"monitor\"")]
            public static readonly dom.Literals.Types.monitor monitor;
            [Template("<self>\"application\"")]
            public static readonly dom.Literals.Types.application application;
            [Template("<self>\"browser\"")]
            public static readonly dom.Literals.Types.browser browser;
            [Template("<self>\"linear\"")]
            public static readonly dom.Literals.Types.linear linear;
            [Template("<self>\"inverse\"")]
            public static readonly dom.Literals.Types.inverse inverse;
            [Template("<self>\"exponential\"")]
            public static readonly dom.Literals.Types.exponential exponential;
            [Template("<self>\"network\"")]
            public static readonly dom.Literals.Types.network network;
            [Template("<self>\"decode\"")]
            public static readonly dom.Literals.Types.decode decode;
            [Template("<self>\"character\"")]
            public static readonly dom.Literals.Types.character character;
            [Template("<self>\"word\"")]
            public static readonly dom.Literals.Types.word word;
            [Template("<self>\"sentence\"")]
            public static readonly dom.Literals.Types.sentence sentence;
            [Template("<self>\"textedit\"")]
            public static readonly dom.Literals.Types.textedit textedit;
            [Template("<self>\"left\"")]
            public static readonly dom.Literals.Types.left left;
            [Template("<self>\"right\"")]
            public static readonly dom.Literals.Types.right right;
            [Template("<self>\"vibration\"")]
            public static readonly dom.Literals.Types.vibration vibration;
            [Template("<self>\"mouse\"")]
            public static readonly dom.Literals.Types.mouse mouse;
            [Template("<self>\"keyboard\"")]
            public static readonly dom.Literals.Types.keyboard keyboard;
            [Template("<self>\"gamepad\"")]
            public static readonly dom.Literals.Types.gamepad gamepad;
            [Template("<self>\"standard\"")]
            public static readonly dom.Literals.Types.standard standard;
            [Template("<self>\"next\"")]
            public static readonly dom.Literals.Types.next next;
            [Template("<self>\"nextunique\"")]
            public static readonly dom.Literals.Types.nextunique nextunique;
            [Template("<self>\"prev\"")]
            public static readonly dom.Literals.Types.prev prev;
            [Template("<self>\"prevunique\"")]
            public static readonly dom.Literals.Types.prevunique prevunique;
            [Template("<self>\"pending\"")]
            public static readonly dom.Literals.Types.pending pending;
            [Template("<self>\"done\"")]
            public static readonly dom.Literals.Types.done done;
            [Template("<self>\"readonly\"")]
            public static readonly dom.Literals.Types.@readonly @readonly;
            [Template("<self>\"readwrite\"")]
            public static readonly dom.Literals.Types.readwrite readwrite;
            [Template("<self>\"versionchange\"")]
            public static readonly dom.Literals.Types.versionchange versionchange;
            [Template("<self>\"raw\"")]
            public static readonly dom.Literals.Types.raw raw;
            [Template("<self>\"spki\"")]
            public static readonly dom.Literals.Types.spki spki;
            [Template("<self>\"pkcs8\"")]
            public static readonly dom.Literals.Types.pkcs8 pkcs8;
            [Template("<self>\"jwk\"")]
            public static readonly dom.Literals.Types.jwk jwk;
            [Template("<self>\"public\"")]
            public static readonly dom.Literals.Types.@public @public;
            [Template("<self>\"private\"")]
            public static readonly dom.Literals.Types.@private @private;
            [Template("<self>\"secret\"")]
            public static readonly dom.Literals.Types.secret secret;
            [Template("<self>\"encrypt\"")]
            public static readonly dom.Literals.Types.encrypt encrypt;
            [Template("<self>\"decrypt\"")]
            public static readonly dom.Literals.Types.decrypt decrypt;
            [Template("<self>\"sign\"")]
            public static readonly dom.Literals.Types.sign sign;
            [Template("<self>\"verify\"")]
            public static readonly dom.Literals.Types.verify verify;
            [Template("<self>\"deriveKey\"")]
            public static readonly dom.Literals.Types.deriveKey deriveKey;
            [Template("<self>\"deriveBits\"")]
            public static readonly dom.Literals.Types.deriveBits deriveBits;
            [Template("<self>\"wrapKey\"")]
            public static readonly dom.Literals.Types.wrapKey wrapKey;
            [Template("<self>\"unwrapKey\"")]
            public static readonly dom.Literals.Types.unwrapKey unwrapKey;
            [Template("<self>\"inactive\"")]
            public static readonly dom.Literals.Types.inactive inactive;
            [Template("<self>\"active\"")]
            public static readonly dom.Literals.Types.active active;
            [Template("<self>\"disambiguation\"")]
            public static readonly dom.Literals.Types.disambiguation disambiguation;
            [Template("<self>\"FIDO_2_0\"")]
            public static readonly dom.Literals.Types.FIDO_2_0 FIDO_2_0;
            [Template("<self>\"os\"")]
            public static readonly dom.Literals.Types.os os;
            [Template("<self>\"stun\"")]
            public static readonly dom.Literals.Types.stun stun;
            [Template("<self>\"turn\"")]
            public static readonly dom.Literals.Types.turn turn;
            [Template("<self>\"peer-derived\"")]
            public static readonly dom.Literals.Types.peer_derived peer_derived;
            [Template("<self>\"failed\"")]
            public static readonly dom.Literals.Types.failed failed;
            [Template("<self>\"direct\"")]
            public static readonly dom.Literals.Types.direct direct;
            [Template("<self>\"relay\"")]
            public static readonly dom.Literals.Types.relay relay;
            [Template("<self>\"description\"")]
            public static readonly dom.Literals.Types.description description;
            [Template("<self>\"localclientevent\"")]
            public static readonly dom.Literals.Types.localclientevent localclientevent;
            [Template("<self>\"inbound-network\"")]
            public static readonly dom.Literals.Types.inbound_network inbound_network;
            [Template("<self>\"outbound-network\"")]
            public static readonly dom.Literals.Types.outbound_network outbound_network;
            [Template("<self>\"inbound-payload\"")]
            public static readonly dom.Literals.Types.inbound_payload inbound_payload;
            [Template("<self>\"outbound-payload\"")]
            public static readonly dom.Literals.Types.outbound_payload outbound_payload;
            [Template("<self>\"transportdiagnostics\"")]
            public static readonly dom.Literals.Types.transportdiagnostics transportdiagnostics;
            [Template("<self>\"Embedded\"")]
            public static readonly dom.Literals.Types.Embedded Embedded;
            [Template("<self>\"USB\"")]
            public static readonly dom.Literals.Types.USB USB;
            [Template("<self>\"NFC\"")]
            public static readonly dom.Literals.Types.NFC NFC;
            [Template("<self>\"BT\"")]
            public static readonly dom.Literals.Types.BT BT;
            [Template("<self>\"unknown\"")]
            public static readonly dom.Literals.Types.unknown unknown;
            [Template("<self>\"defer\"")]
            public static readonly dom.Literals.Types.defer defer;
            [Template("<self>\"allow\"")]
            public static readonly dom.Literals.Types.allow allow;
            [Template("<self>\"deny\"")]
            public static readonly dom.Literals.Types.deny deny;
            [Template("<self>\"geolocation\"")]
            public static readonly dom.Literals.Types.geolocation geolocation;
            [Template("<self>\"unlimitedIndexedDBQuota\"")]
            public static readonly dom.Literals.Types.unlimitedIndexedDBQuota unlimitedIndexedDBQuota;
            [Template("<self>\"media\"")]
            public static readonly dom.Literals.Types.media media;
            [Template("<self>\"pointerlock\"")]
            public static readonly dom.Literals.Types.pointerlock pointerlock;
            [Template("<self>\"webnotifications\"")]
            public static readonly dom.Literals.Types.webnotifications webnotifications;
            [Template("<self>\"audioinput\"")]
            public static readonly dom.Literals.Types.audioinput audioinput;
            [Template("<self>\"audiooutput\"")]
            public static readonly dom.Literals.Types.audiooutput audiooutput;
            [Template("<self>\"videoinput\"")]
            public static readonly dom.Literals.Types.videoinput videoinput;
            [Template("<self>\"license-request\"")]
            public static readonly dom.Literals.Types.license_request license_request;
            [Template("<self>\"license-renewal\"")]
            public static readonly dom.Literals.Types.license_renewal license_renewal;
            [Template("<self>\"license-release\"")]
            public static readonly dom.Literals.Types.license_release license_release;
            [Template("<self>\"individualization-request\"")]
            public static readonly dom.Literals.Types.individualization_request individualization_request;
            [Template("<self>\"temporary\"")]
            public static readonly dom.Literals.Types.temporary temporary;
            [Template("<self>\"persistent-license\"")]
            public static readonly dom.Literals.Types.persistent_license persistent_license;
            [Template("<self>\"persistent-release-message\"")]
            public static readonly dom.Literals.Types.persistent_release_message persistent_release_message;
            [Template("<self>\"usable\"")]
            public static readonly dom.Literals.Types.usable usable;
            [Template("<self>\"expired\"")]
            public static readonly dom.Literals.Types.expired expired;
            [Template("<self>\"output-downscaled\"")]
            public static readonly dom.Literals.Types.output_downscaled output_downscaled;
            [Template("<self>\"output-not-allowed\"")]
            public static readonly dom.Literals.Types.output_not_allowed output_not_allowed;
            [Template("<self>\"status-pending\"")]
            public static readonly dom.Literals.Types.status_pending status_pending;
            [Template("<self>\"internal-error\"")]
            public static readonly dom.Literals.Types.internal_error internal_error;
            [Template("<self>\"required\"")]
            public static readonly dom.Literals.Types.required required;
            [Template("<self>\"optional\"")]
            public static readonly dom.Literals.Types.optional optional;
            [Template("<self>\"not-allowed\"")]
            public static readonly dom.Literals.Types.not_allowed not_allowed;
            [Template("<self>\"live\"")]
            public static readonly dom.Literals.Types.live live;
            [Template("<self>\"ended\"")]
            public static readonly dom.Literals.Types.ended ended;
            [Template("<self>\"up\"")]
            public static readonly dom.Literals.Types.up up;
            [Template("<self>\"down\"")]
            public static readonly dom.Literals.Types.down down;
            [Template("<self>\"navigate\"")]
            public static readonly dom.Literals.Types.navigate navigate;
            [Template("<self>\"reload\"")]
            public static readonly dom.Literals.Types.reload reload;
            [Template("<self>\"back_forward\"")]
            public static readonly dom.Literals.Types.back_forward back_forward;
            [Template("<self>\"prerender\"")]
            public static readonly dom.Literals.Types.prerender prerender;
            [Template("<self>\"ltr\"")]
            public static readonly dom.Literals.Types.ltr ltr;
            [Template("<self>\"rtl\"")]
            public static readonly dom.Literals.Types.rtl rtl;
            [Template("<self>\"default\"")]
            public static readonly dom.Literals.Types.@default @default;
            [Template("<self>\"denied\"")]
            public static readonly dom.Literals.Types.denied denied;
            [Template("<self>\"granted\"")]
            public static readonly dom.Literals.Types.granted granted;
            [Template("<self>\"sine\"")]
            public static readonly dom.Literals.Types.sine sine;
            [Template("<self>\"square\"")]
            public static readonly dom.Literals.Types.square square;
            [Template("<self>\"sawtooth\"")]
            public static readonly dom.Literals.Types.sawtooth sawtooth;
            [Template("<self>\"triangle\"")]
            public static readonly dom.Literals.Types.triangle triangle;
            [Template("<self>\"custom\"")]
            public static readonly dom.Literals.Types.custom custom;
            [Template("<self>\"none\"")]
            public static readonly dom.Literals.Types.none none;
            [Template("<self>\"2x\"")]
            public static readonly dom.Literals.Types._2x _2x;
            [Template("<self>\"4x\"")]
            public static readonly dom.Literals.Types._4x _4x;
            [Template("<self>\"equalpower\"")]
            public static readonly dom.Literals.Types.equalpower equalpower;
            [Template("<self>\"HRTF\"")]
            public static readonly dom.Literals.Types.HRTF HRTF;
            [Template("<self>\"success\"")]
            public static readonly dom.Literals.Types.success success;
            [Template("<self>\"fail\"")]
            public static readonly dom.Literals.Types.fail fail;
            [Template("<self>\"shipping\"")]
            public static readonly dom.Literals.Types.shipping shipping;
            [Template("<self>\"delivery\"")]
            public static readonly dom.Literals.Types.delivery delivery;
            [Template("<self>\"pickup\"")]
            public static readonly dom.Literals.Types.pickup pickup;
            [Template("<self>\"p256dh\"")]
            public static readonly dom.Literals.Types.p256dh p256dh;
            [Template("<self>\"auth\"")]
            public static readonly dom.Literals.Types.auth auth;
            [Template("<self>\"prompt\"")]
            public static readonly dom.Literals.Types.prompt prompt;
            [Template("<self>\"max-compat\"")]
            public static readonly dom.Literals.Types.max_compat max_compat;
            [Template("<self>\"max-bundle\"")]
            public static readonly dom.Literals.Types.max_bundle max_bundle;
            [Template("<self>\"maintain-framerate\"")]
            public static readonly dom.Literals.Types.maintain_framerate maintain_framerate;
            [Template("<self>\"maintain-resolution\"")]
            public static readonly dom.Literals.Types.maintain_resolution maintain_resolution;
            [Template("<self>\"client\"")]
            public static readonly dom.Literals.Types.client client;
            [Template("<self>\"server\"")]
            public static readonly dom.Literals.Types.server server;
            [Template("<self>\"new\"")]
            public static readonly dom.Literals.Types.@new @new;
            [Template("<self>\"connecting\"")]
            public static readonly dom.Literals.Types.connecting connecting;
            [Template("<self>\"connected\"")]
            public static readonly dom.Literals.Types.connected connected;
            [Template("<self>\"host\"")]
            public static readonly dom.Literals.Types.host host;
            [Template("<self>\"srflx\"")]
            public static readonly dom.Literals.Types.srflx srflx;
            [Template("<self>\"prflx\"")]
            public static readonly dom.Literals.Types.prflx prflx;
            [Template("<self>\"RTP\"")]
            public static readonly dom.Literals.Types.RTP RTP;
            [Template("<self>\"RTCP\"")]
            public static readonly dom.Literals.Types.RTCP RTCP;
            [Template("<self>\"checking\"")]
            public static readonly dom.Literals.Types.checking checking;
            [Template("<self>\"completed\"")]
            public static readonly dom.Literals.Types.completed completed;
            [Template("<self>\"disconnected\"")]
            public static readonly dom.Literals.Types.disconnected disconnected;
            [Template("<self>\"nohost\"")]
            public static readonly dom.Literals.Types.nohost nohost;
            [Template("<self>\"gathering\"")]
            public static readonly dom.Literals.Types.gathering gathering;
            [Template("<self>\"complete\"")]
            public static readonly dom.Literals.Types.complete complete;
            [Template("<self>\"udp\"")]
            public static readonly dom.Literals.Types.udp udp;
            [Template("<self>\"tcp\"")]
            public static readonly dom.Literals.Types.tcp tcp;
            [Template("<self>\"controlling\"")]
            public static readonly dom.Literals.Types.controlling controlling;
            [Template("<self>\"controlled\"")]
            public static readonly dom.Literals.Types.controlled controlled;
            [Template("<self>\"passive\"")]
            public static readonly dom.Literals.Types.passive passive;
            [Template("<self>\"so\"")]
            public static readonly dom.Literals.Types.so so;
            [Template("<self>\"offer\"")]
            public static readonly dom.Literals.Types.offer offer;
            [Template("<self>\"pranswer\"")]
            public static readonly dom.Literals.Types.pranswer pranswer;
            [Template("<self>\"answer\"")]
            public static readonly dom.Literals.Types.answer answer;
            [Template("<self>\"stable\"")]
            public static readonly dom.Literals.Types.stable stable;
            [Template("<self>\"have-local-offer\"")]
            public static readonly dom.Literals.Types.have_local_offer have_local_offer;
            [Template("<self>\"have-remote-offer\"")]
            public static readonly dom.Literals.Types.have_remote_offer have_remote_offer;
            [Template("<self>\"have-local-pranswer\"")]
            public static readonly dom.Literals.Types.have_local_pranswer have_local_pranswer;
            [Template("<self>\"have-remote-pranswer\"")]
            public static readonly dom.Literals.Types.have_remote_pranswer have_remote_pranswer;
            [Template("<self>\"frozen\"")]
            public static readonly dom.Literals.Types.frozen frozen;
            [Template("<self>\"waiting\"")]
            public static readonly dom.Literals.Types.waiting waiting;
            [Template("<self>\"inprogress\"")]
            public static readonly dom.Literals.Types.inprogress inprogress;
            [Template("<self>\"succeeded\"")]
            public static readonly dom.Literals.Types.succeeded succeeded;
            [Template("<self>\"cancelled\"")]
            public static readonly dom.Literals.Types.cancelled cancelled;
            [Template("<self>\"serverreflexive\"")]
            public static readonly dom.Literals.Types.serverreflexive serverreflexive;
            [Template("<self>\"peerreflexive\"")]
            public static readonly dom.Literals.Types.peerreflexive peerreflexive;
            [Template("<self>\"relayed\"")]
            public static readonly dom.Literals.Types.relayed relayed;
            [Template("<self>\"inboundrtp\"")]
            public static readonly dom.Literals.Types.inboundrtp inboundrtp;
            [Template("<self>\"outboundrtp\"")]
            public static readonly dom.Literals.Types.outboundrtp outboundrtp;
            [Template("<self>\"session\"")]
            public static readonly dom.Literals.Types.session session;
            [Template("<self>\"datachannel\"")]
            public static readonly dom.Literals.Types.datachannel datachannel;
            [Template("<self>\"track\"")]
            public static readonly dom.Literals.Types.track track;
            [Template("<self>\"transport\"")]
            public static readonly dom.Literals.Types.transport transport;
            [Template("<self>\"candidatepair\"")]
            public static readonly dom.Literals.Types.candidatepair candidatepair;
            [Template("<self>\"localcandidate\"")]
            public static readonly dom.Literals.Types.localcandidate localcandidate;
            [Template("<self>\"remotecandidate\"")]
            public static readonly dom.Literals.Types.remotecandidate remotecandidate;
            [Template("<self>\"open\"")]
            public static readonly dom.Literals.Types.open open;
            [Template("<self>\"no-referrer\"")]
            public static readonly dom.Literals.Types.no_referrer no_referrer;
            [Template("<self>\"no-referrer-when-downgrade\"")]
            public static readonly dom.Literals.Types.no_referrer_when_downgrade no_referrer_when_downgrade;
            [Template("<self>\"origin-only\"")]
            public static readonly dom.Literals.Types.origin_only origin_only;
            [Template("<self>\"origin-when-cross-origin\"")]
            public static readonly dom.Literals.Types.origin_when_cross_origin origin_when_cross_origin;
            [Template("<self>\"unsafe-url\"")]
            public static readonly dom.Literals.Types.unsafe_url unsafe_url;
            [Template("<self>\"no-store\"")]
            public static readonly dom.Literals.Types.no_store no_store;
            [Template("<self>\"no-cache\"")]
            public static readonly dom.Literals.Types.no_cache no_cache;
            [Template("<self>\"force-cache\"")]
            public static readonly dom.Literals.Types.force_cache force_cache;
            [Template("<self>\"omit\"")]
            public static readonly dom.Literals.Types.omit omit;
            [Template("<self>\"same-origin\"")]
            public static readonly dom.Literals.Types.same_origin same_origin;
            [Template("<self>\"include\"")]
            public static readonly dom.Literals.Types.include include;
            [Template("<self>\"document\"")]
            public static readonly dom.Literals.Types.document document;
            [Template("<self>\"subresource\"")]
            public static readonly dom.Literals.Types.subresource subresource;
            [Template("<self>\"no-cors\"")]
            public static readonly dom.Literals.Types.no_cors no_cors;
            [Template("<self>\"cors\"")]
            public static readonly dom.Literals.Types.cors cors;
            [Template("<self>\"follow\"")]
            public static readonly dom.Literals.Types.follow follow;
            [Template("<self>\"error\"")]
            public static readonly dom.Literals.Types.error error;
            [Template("<self>\"audio\"")]
            public static readonly dom.Literals.Types.audio audio;
            [Template("<self>\"font\"")]
            public static readonly dom.Literals.Types.font font;
            [Template("<self>\"image\"")]
            public static readonly dom.Literals.Types.image image;
            [Template("<self>\"script\"")]
            public static readonly dom.Literals.Types.script script;
            [Template("<self>\"style\"")]
            public static readonly dom.Literals.Types.style style;
            [Template("<self>\"video\"")]
            public static readonly dom.Literals.Types.video video;
            [Template("<self>\"basic\"")]
            public static readonly dom.Literals.Types.basic basic;
            [Template("<self>\"opaque\"")]
            public static readonly dom.Literals.Types.opaque opaque;
            [Template("<self>\"opaqueredirect\"")]
            public static readonly dom.Literals.Types.opaqueredirect opaqueredirect;
            [Template("<self>\"ScopedCred\"")]
            public static readonly dom.Literals.Types.ScopedCred ScopedCred;
            [Template("<self>\"installing\"")]
            public static readonly dom.Literals.Types.installing installing;
            [Template("<self>\"installed\"")]
            public static readonly dom.Literals.Types.installed installed;
            [Template("<self>\"activating\"")]
            public static readonly dom.Literals.Types.activating activating;
            [Template("<self>\"activated\"")]
            public static readonly dom.Literals.Types.activated activated;
            [Template("<self>\"redundant\"")]
            public static readonly dom.Literals.Types.redundant redundant;
            [Template("<self>\"subtitles\"")]
            public static readonly dom.Literals.Types.subtitles subtitles;
            [Template("<self>\"captions\"")]
            public static readonly dom.Literals.Types.captions captions;
            [Template("<self>\"descriptions\"")]
            public static readonly dom.Literals.Types.descriptions descriptions;
            [Template("<self>\"chapters\"")]
            public static readonly dom.Literals.Types.chapters chapters;
            [Template("<self>\"metadata\"")]
            public static readonly dom.Literals.Types.metadata metadata;
            [Template("<self>\"disabled\"")]
            public static readonly dom.Literals.Types.disabled disabled;
            [Template("<self>\"hidden\"")]
            public static readonly dom.Literals.Types.hidden hidden;
            [Template("<self>\"showing\"")]
            public static readonly dom.Literals.Types.showing showing;
            [Template("<self>\"usb\"")]
            public static readonly dom.Literals.Types.usb usb;
            [Template("<self>\"nfc\"")]
            public static readonly dom.Literals.Types.nfc nfc;
            [Template("<self>\"ble\"")]
            public static readonly dom.Literals.Types.ble ble;
            [Template("<self>\"mounted\"")]
            public static readonly dom.Literals.Types.mounted mounted;
            [Template("<self>\"navigation\"")]
            public static readonly dom.Literals.Types.navigation navigation;
            [Template("<self>\"requested\"")]
            public static readonly dom.Literals.Types.requested requested;
            [Template("<self>\"unmounted\"")]
            public static readonly dom.Literals.Types.unmounted unmounted;
            [Template("<self>\"user\"")]
            public static readonly dom.Literals.Types.user user;
            [Template("<self>\"environment\"")]
            public static readonly dom.Literals.Types.environment environment;
            [Template("<self>\"visible\"")]
            public static readonly dom.Literals.Types.visible visible;
            [Template("<self>\"unloaded\"")]
            public static readonly dom.Literals.Types.unloaded unloaded;
            [Template("<self>\"json\"")]
            public static readonly dom.Literals.Types.json json;
            [Template("<self>\"text\"")]
            public static readonly dom.Literals.Types.text text;
            [Template("<self>\"idle\"")]
            public static readonly dom.Literals.Types.idle idle;
            [Template("<self>\"paused\"")]
            public static readonly dom.Literals.Types.paused paused;
            [Template("<self>\"finished\"")]
            public static readonly dom.Literals.Types.finished finished;
            [Template("<self>\"normal\"")]
            public static readonly dom.Literals.Types.normal normal;
            [Template("<self>\"reverse\"")]
            public static readonly dom.Literals.Types.reverse reverse;
            [Template("<self>\"alternate\"")]
            public static readonly dom.Literals.Types.alternate alternate;
            [Template("<self>\"alternate-reverse\"")]
            public static readonly dom.Literals.Types.alternate_reverse alternate_reverse;
            [Template("<self>\"forwards\"")]
            public static readonly dom.Literals.Types.forwards forwards;
            [Template("<self>\"backwards\"")]
            public static readonly dom.Literals.Types.backwards backwards;
            [Template("<self>\"both\"")]
            public static readonly dom.Literals.Types.both both;
            [Template("<self>\"http://www.w3.org/1999/xhtml\"")]
            public static readonly dom.Literals.Types.http_SlashSlashwww_w3_orgSlash1999Slashxhtml http_SlashSlashwww_w3_orgSlash1999Slashxhtml;
            [Template("<self>\"http://www.w3.org/2000/svg\"")]
            public static readonly dom.Literals.Types.http_SlashSlashwww_w3_orgSlash2000Slashsvg http_SlashSlashwww_w3_orgSlash2000Slashsvg;
            [Template("<self>\"a\"")]
            public static readonly dom.Literals.Types.a a;
            [Template("<self>\"circle\"")]
            public static readonly dom.Literals.Types.circle circle;
            [Template("<self>\"clipPath\"")]
            public static readonly dom.Literals.Types.clipPath clipPath;
            [Template("<self>\"componentTransferFunction\"")]
            public static readonly dom.Literals.Types.componentTransferFunction componentTransferFunction;
            [Template("<self>\"defs\"")]
            public static readonly dom.Literals.Types.defs defs;
            [Template("<self>\"desc\"")]
            public static readonly dom.Literals.Types.desc desc;
            [Template("<self>\"ellipse\"")]
            public static readonly dom.Literals.Types.ellipse ellipse;
            [Template("<self>\"feBlend\"")]
            public static readonly dom.Literals.Types.feBlend feBlend;
            [Template("<self>\"feColorMatrix\"")]
            public static readonly dom.Literals.Types.feColorMatrix feColorMatrix;
            [Template("<self>\"feComponentTransfer\"")]
            public static readonly dom.Literals.Types.feComponentTransfer feComponentTransfer;
            [Template("<self>\"feComposite\"")]
            public static readonly dom.Literals.Types.feComposite feComposite;
            [Template("<self>\"feConvolveMatrix\"")]
            public static readonly dom.Literals.Types.feConvolveMatrix feConvolveMatrix;
            [Template("<self>\"feDiffuseLighting\"")]
            public static readonly dom.Literals.Types.feDiffuseLighting feDiffuseLighting;
            [Template("<self>\"feDisplacementMap\"")]
            public static readonly dom.Literals.Types.feDisplacementMap feDisplacementMap;
            [Template("<self>\"feDistantLight\"")]
            public static readonly dom.Literals.Types.feDistantLight feDistantLight;
            [Template("<self>\"feFlood\"")]
            public static readonly dom.Literals.Types.feFlood feFlood;
            [Template("<self>\"feFuncA\"")]
            public static readonly dom.Literals.Types.feFuncA feFuncA;
            [Template("<self>\"feFuncB\"")]
            public static readonly dom.Literals.Types.feFuncB feFuncB;
            [Template("<self>\"feFuncG\"")]
            public static readonly dom.Literals.Types.feFuncG feFuncG;
            [Template("<self>\"feFuncR\"")]
            public static readonly dom.Literals.Types.feFuncR feFuncR;
            [Template("<self>\"feGaussianBlur\"")]
            public static readonly dom.Literals.Types.feGaussianBlur feGaussianBlur;
            [Template("<self>\"feImage\"")]
            public static readonly dom.Literals.Types.feImage feImage;
            [Template("<self>\"feMerge\"")]
            public static readonly dom.Literals.Types.feMerge feMerge;
            [Template("<self>\"feMergeNode\"")]
            public static readonly dom.Literals.Types.feMergeNode feMergeNode;
            [Template("<self>\"feMorphology\"")]
            public static readonly dom.Literals.Types.feMorphology feMorphology;
            [Template("<self>\"feOffset\"")]
            public static readonly dom.Literals.Types.feOffset feOffset;
            [Template("<self>\"fePointLight\"")]
            public static readonly dom.Literals.Types.fePointLight fePointLight;
            [Template("<self>\"feSpecularLighting\"")]
            public static readonly dom.Literals.Types.feSpecularLighting feSpecularLighting;
            [Template("<self>\"feSpotLight\"")]
            public static readonly dom.Literals.Types.feSpotLight feSpotLight;
            [Template("<self>\"feTile\"")]
            public static readonly dom.Literals.Types.feTile feTile;
            [Template("<self>\"feTurbulence\"")]
            public static readonly dom.Literals.Types.feTurbulence feTurbulence;
            [Template("<self>\"filter\"")]
            public static readonly dom.Literals.Types.filter filter;
            [Template("<self>\"foreignObject\"")]
            public static readonly dom.Literals.Types.foreignObject foreignObject;
            [Template("<self>\"g\"")]
            public static readonly dom.Literals.Types.g g;
            [Template("<self>\"gradient\"")]
            public static readonly dom.Literals.Types.gradient gradient;
            [Template("<self>\"line\"")]
            public static readonly dom.Literals.Types.line line;
            [Template("<self>\"linearGradient\"")]
            public static readonly dom.Literals.Types.linearGradient linearGradient;
            [Template("<self>\"marker\"")]
            public static readonly dom.Literals.Types.marker marker;
            [Template("<self>\"mask\"")]
            public static readonly dom.Literals.Types.mask mask;
            [Template("<self>\"path\"")]
            public static readonly dom.Literals.Types.path path;
            [Template("<self>\"pattern\"")]
            public static readonly dom.Literals.Types.pattern pattern;
            [Template("<self>\"polygon\"")]
            public static readonly dom.Literals.Types.polygon polygon;
            [Template("<self>\"polyline\"")]
            public static readonly dom.Literals.Types.polyline polyline;
            [Template("<self>\"radialGradient\"")]
            public static readonly dom.Literals.Types.radialGradient radialGradient;
            [Template("<self>\"rect\"")]
            public static readonly dom.Literals.Types.rect rect;
            [Template("<self>\"svg\"")]
            public static readonly dom.Literals.Types.svg svg;
            [Template("<self>\"stop\"")]
            public static readonly dom.Literals.Types.stop stop;
            [Template("<self>\"switch\"")]
            public static readonly dom.Literals.Types.@switch @switch;
            [Template("<self>\"symbol\"")]
            public static readonly dom.Literals.Types.symbol symbol;
            [Template("<self>\"tspan\"")]
            public static readonly dom.Literals.Types.tspan tspan;
            [Template("<self>\"textContent\"")]
            public static readonly dom.Literals.Types.textContent textContent;
            [Template("<self>\"textPath\"")]
            public static readonly dom.Literals.Types.textPath textPath;
            [Template("<self>\"textPositioning\"")]
            public static readonly dom.Literals.Types.textPositioning textPositioning;
            [Template("<self>\"title\"")]
            public static readonly dom.Literals.Types.title title;
            [Template("<self>\"use\"")]
            public static readonly dom.Literals.Types.use use;
            [Template("<self>\"view\"")]
            public static readonly dom.Literals.Types.view view;
            [Template("<self>\"AnimationEvent\"")]
            public static readonly dom.Literals.Types.AnimationEvent AnimationEvent;
            [Template("<self>\"AnimationPlaybackEvent\"")]
            public static readonly dom.Literals.Types.AnimationPlaybackEvent AnimationPlaybackEvent;
            [Template("<self>\"AudioProcessingEvent\"")]
            public static readonly dom.Literals.Types.AudioProcessingEvent AudioProcessingEvent;
            [Template("<self>\"BeforeUnloadEvent\"")]
            public static readonly dom.Literals.Types.BeforeUnloadEvent BeforeUnloadEvent;
            [Template("<self>\"ClipboardEvent\"")]
            public static readonly dom.Literals.Types.ClipboardEvent ClipboardEvent;
            [Template("<self>\"CloseEvent\"")]
            public static readonly dom.Literals.Types.CloseEvent CloseEvent;
            [Template("<self>\"CompositionEvent\"")]
            public static readonly dom.Literals.Types.CompositionEvent CompositionEvent;
            [Template("<self>\"CustomEvent\"")]
            public static readonly dom.Literals.Types.CustomEvent CustomEvent;
            [Template("<self>\"DeviceLightEvent\"")]
            public static readonly dom.Literals.Types.DeviceLightEvent DeviceLightEvent;
            [Template("<self>\"DeviceMotionEvent\"")]
            public static readonly dom.Literals.Types.DeviceMotionEvent DeviceMotionEvent;
            [Template("<self>\"DeviceOrientationEvent\"")]
            public static readonly dom.Literals.Types.DeviceOrientationEvent DeviceOrientationEvent;
            [Template("<self>\"DragEvent\"")]
            public static readonly dom.Literals.Types.DragEvent DragEvent;
            [Template("<self>\"ErrorEvent\"")]
            public static readonly dom.Literals.Types.ErrorEvent ErrorEvent;
            [Template("<self>\"Event\"")]
            public static readonly dom.Literals.Types.Event Event;
            [Template("<self>\"Events\"")]
            public static readonly dom.Literals.Types.Events Events;
            [Template("<self>\"FocusEvent\"")]
            public static readonly dom.Literals.Types.FocusEvent FocusEvent;
            [Template("<self>\"FocusNavigationEvent\"")]
            public static readonly dom.Literals.Types.FocusNavigationEvent FocusNavigationEvent;
            [Template("<self>\"GamepadEvent\"")]
            public static readonly dom.Literals.Types.GamepadEvent GamepadEvent;
            [Template("<self>\"HashChangeEvent\"")]
            public static readonly dom.Literals.Types.HashChangeEvent HashChangeEvent;
            [Template("<self>\"IDBVersionChangeEvent\"")]
            public static readonly dom.Literals.Types.IDBVersionChangeEvent IDBVersionChangeEvent;
            [Template("<self>\"KeyboardEvent\"")]
            public static readonly dom.Literals.Types.KeyboardEvent KeyboardEvent;
            [Template("<self>\"ListeningStateChangedEvent\"")]
            public static readonly dom.Literals.Types.ListeningStateChangedEvent ListeningStateChangedEvent;
            [Template("<self>\"MSDCCEvent\"")]
            public static readonly dom.Literals.Types.MSDCCEvent MSDCCEvent;
            [Template("<self>\"MSDSHEvent\"")]
            public static readonly dom.Literals.Types.MSDSHEvent MSDSHEvent;
            [Template("<self>\"MSMediaKeyMessageEvent\"")]
            public static readonly dom.Literals.Types.MSMediaKeyMessageEvent MSMediaKeyMessageEvent;
            [Template("<self>\"MSMediaKeyNeededEvent\"")]
            public static readonly dom.Literals.Types.MSMediaKeyNeededEvent MSMediaKeyNeededEvent;
            [Template("<self>\"MediaEncryptedEvent\"")]
            public static readonly dom.Literals.Types.MediaEncryptedEvent MediaEncryptedEvent;
            [Template("<self>\"MediaKeyMessageEvent\"")]
            public static readonly dom.Literals.Types.MediaKeyMessageEvent MediaKeyMessageEvent;
            [Template("<self>\"MediaStreamErrorEvent\"")]
            public static readonly dom.Literals.Types.MediaStreamErrorEvent MediaStreamErrorEvent;
            [Template("<self>\"MediaStreamEvent\"")]
            public static readonly dom.Literals.Types.MediaStreamEvent MediaStreamEvent;
            [Template("<self>\"MediaStreamTrackEvent\"")]
            public static readonly dom.Literals.Types.MediaStreamTrackEvent MediaStreamTrackEvent;
            [Template("<self>\"MessageEvent\"")]
            public static readonly dom.Literals.Types.MessageEvent MessageEvent;
            [Template("<self>\"MouseEvent\"")]
            public static readonly dom.Literals.Types.MouseEvent MouseEvent;
            [Template("<self>\"MouseEvents\"")]
            public static readonly dom.Literals.Types.MouseEvents MouseEvents;
            [Template("<self>\"MutationEvent\"")]
            public static readonly dom.Literals.Types.MutationEvent MutationEvent;
            [Template("<self>\"MutationEvents\"")]
            public static readonly dom.Literals.Types.MutationEvents MutationEvents;
            [Template("<self>\"OfflineAudioCompletionEvent\"")]
            public static readonly dom.Literals.Types.OfflineAudioCompletionEvent OfflineAudioCompletionEvent;
            [Template("<self>\"OverflowEvent\"")]
            public static readonly dom.Literals.Types.OverflowEvent OverflowEvent;
            [Template("<self>\"PageTransitionEvent\"")]
            public static readonly dom.Literals.Types.PageTransitionEvent PageTransitionEvent;
            [Template("<self>\"PaymentRequestUpdateEvent\"")]
            public static readonly dom.Literals.Types.PaymentRequestUpdateEvent PaymentRequestUpdateEvent;
            [Template("<self>\"PermissionRequestedEvent\"")]
            public static readonly dom.Literals.Types.PermissionRequestedEvent PermissionRequestedEvent;
            [Template("<self>\"PointerEvent\"")]
            public static readonly dom.Literals.Types.PointerEvent PointerEvent;
            [Template("<self>\"PopStateEvent\"")]
            public static readonly dom.Literals.Types.PopStateEvent PopStateEvent;
            [Template("<self>\"ProgressEvent\"")]
            public static readonly dom.Literals.Types.ProgressEvent ProgressEvent;
            [Template("<self>\"PromiseRejectionEvent\"")]
            public static readonly dom.Literals.Types.PromiseRejectionEvent PromiseRejectionEvent;
            [Template("<self>\"RTCDTMFToneChangeEvent\"")]
            public static readonly dom.Literals.Types.RTCDTMFToneChangeEvent RTCDTMFToneChangeEvent;
            [Template("<self>\"RTCDtlsTransportStateChangedEvent\"")]
            public static readonly dom.Literals.Types.RTCDtlsTransportStateChangedEvent RTCDtlsTransportStateChangedEvent;
            [Template("<self>\"RTCIceCandidatePairChangedEvent\"")]
            public static readonly dom.Literals.Types.RTCIceCandidatePairChangedEvent RTCIceCandidatePairChangedEvent;
            [Template("<self>\"RTCIceGathererEvent\"")]
            public static readonly dom.Literals.Types.RTCIceGathererEvent RTCIceGathererEvent;
            [Template("<self>\"RTCIceTransportStateChangedEvent\"")]
            public static readonly dom.Literals.Types.RTCIceTransportStateChangedEvent RTCIceTransportStateChangedEvent;
            [Template("<self>\"RTCPeerConnectionIceEvent\"")]
            public static readonly dom.Literals.Types.RTCPeerConnectionIceEvent RTCPeerConnectionIceEvent;
            [Template("<self>\"RTCSsrcConflictEvent\"")]
            public static readonly dom.Literals.Types.RTCSsrcConflictEvent RTCSsrcConflictEvent;
            [Template("<self>\"SVGZoomEvent\"")]
            public static readonly dom.Literals.Types.SVGZoomEvent SVGZoomEvent;
            [Template("<self>\"SVGZoomEvents\"")]
            public static readonly dom.Literals.Types.SVGZoomEvents SVGZoomEvents;
            [Template("<self>\"SecurityPolicyViolationEvent\"")]
            public static readonly dom.Literals.Types.SecurityPolicyViolationEvent SecurityPolicyViolationEvent;
            [Template("<self>\"ServiceWorkerMessageEvent\"")]
            public static readonly dom.Literals.Types.ServiceWorkerMessageEvent ServiceWorkerMessageEvent;
            [Template("<self>\"SpeechSynthesisEvent\"")]
            public static readonly dom.Literals.Types.SpeechSynthesisEvent SpeechSynthesisEvent;
            [Template("<self>\"StorageEvent\"")]
            public static readonly dom.Literals.Types.StorageEvent StorageEvent;
            [Template("<self>\"TextEvent\"")]
            public static readonly dom.Literals.Types.TextEvent TextEvent;
            [Template("<self>\"TouchEvent\"")]
            public static readonly dom.Literals.Types.TouchEvent TouchEvent;
            [Template("<self>\"TrackEvent\"")]
            public static readonly dom.Literals.Types.TrackEvent TrackEvent;
            [Template("<self>\"TransitionEvent\"")]
            public static readonly dom.Literals.Types.TransitionEvent TransitionEvent;
            [Template("<self>\"UIEvent\"")]
            public static readonly dom.Literals.Types.UIEvent UIEvent;
            [Template("<self>\"UIEvents\"")]
            public static readonly dom.Literals.Types.UIEvents UIEvents;
            [Template("<self>\"VRDisplayEvent\"")]
            public static readonly dom.Literals.Types.VRDisplayEvent VRDisplayEvent;
            [Template("<self>\"VRDisplayEvent \"")]
            public static readonly dom.Literals.Types.VRDisplayEvent_ VRDisplayEvent_;
            [Template("<self>\"WebGLContextEvent\"")]
            public static readonly dom.Literals.Types.WebGLContextEvent WebGLContextEvent;
            [Template("<self>\"WheelEvent\"")]
            public static readonly dom.Literals.Types.WheelEvent WheelEvent;
            [Template("<self>\"2d\"")]
            public static readonly dom.Literals.Types._2d _2d;
            [Template("<self>\"webgl\"")]
            public static readonly dom.Literals.Types.webgl webgl;
            [Template("<self>\"experimental-webgl\"")]
            public static readonly dom.Literals.Types.experimental_webgl experimental_webgl;
            [Template("<self>\"async\"")]
            public static readonly dom.Literals.Types.async async;
            [Template("<self>\"sync\"")]
            public static readonly dom.Literals.Types.sync sync;
            [Template("<self>\"forward\"")]
            public static readonly dom.Literals.Types.forward forward;
            [Template("<self>\"backward\"")]
            public static readonly dom.Literals.Types.backward backward;
            [Template("<self>\"flipY\"")]
            public static readonly dom.Literals.Types.flipY flipY;
            [Template("<self>\"premultiply\"")]
            public static readonly dom.Literals.Types.premultiply premultiply;
            [Template("<self>\"pixelated\"")]
            public static readonly dom.Literals.Types.pixelated pixelated;
            [Template("<self>\"low\"")]
            public static readonly dom.Literals.Types.low low;
            [Template("<self>\"medium\"")]
            public static readonly dom.Literals.Types.medium medium;
            [Template("<self>\"high\"")]
            public static readonly dom.Literals.Types.high high;
            [Template("<self>\"EXT_blend_minmax\"")]
            public static readonly dom.Literals.Types.EXT_blend_minmax EXT_blend_minmax;
            [Template("<self>\"EXT_texture_filter_anisotropic\"")]
            public static readonly dom.Literals.Types.EXT_texture_filter_anisotropic EXT_texture_filter_anisotropic;
            [Template("<self>\"EXT_frag_depth\"")]
            public static readonly dom.Literals.Types.EXT_frag_depth EXT_frag_depth;
            [Template("<self>\"EXT_shader_texture_lod\"")]
            public static readonly dom.Literals.Types.EXT_shader_texture_lod EXT_shader_texture_lod;
            [Template("<self>\"EXT_sRGB\"")]
            public static readonly dom.Literals.Types.EXT_sRGB EXT_sRGB;
            [Template("<self>\"OES_vertex_array_object\"")]
            public static readonly dom.Literals.Types.OES_vertex_array_object OES_vertex_array_object;
            [Template("<self>\"WEBGL_color_buffer_float\"")]
            public static readonly dom.Literals.Types.WEBGL_color_buffer_float WEBGL_color_buffer_float;
            [Template("<self>\"WEBGL_compressed_texture_astc\"")]
            public static readonly dom.Literals.Types.WEBGL_compressed_texture_astc WEBGL_compressed_texture_astc;
            [Template("<self>\"WEBGL_compressed_texture_s3tc_srgb\"")]
            public static readonly dom.Literals.Types.WEBGL_compressed_texture_s3tc_srgb WEBGL_compressed_texture_s3tc_srgb;
            [Template("<self>\"WEBGL_debug_shaders\"")]
            public static readonly dom.Literals.Types.WEBGL_debug_shaders WEBGL_debug_shaders;
            [Template("<self>\"WEBGL_draw_buffers\"")]
            public static readonly dom.Literals.Types.WEBGL_draw_buffers WEBGL_draw_buffers;
            [Template("<self>\"WEBGL_lose_context\"")]
            public static readonly dom.Literals.Types.WEBGL_lose_context WEBGL_lose_context;
            [Template("<self>\"WEBGL_depth_texture\"")]
            public static readonly dom.Literals.Types.WEBGL_depth_texture WEBGL_depth_texture;
            [Template("<self>\"WEBGL_debug_renderer_info\"")]
            public static readonly dom.Literals.Types.WEBGL_debug_renderer_info WEBGL_debug_renderer_info;
            [Template("<self>\"WEBGL_compressed_texture_s3tc\"")]
            public static readonly dom.Literals.Types.WEBGL_compressed_texture_s3tc WEBGL_compressed_texture_s3tc;
            [Template("<self>\"OES_texture_half_float_linear\"")]
            public static readonly dom.Literals.Types.OES_texture_half_float_linear OES_texture_half_float_linear;
            [Template("<self>\"OES_texture_half_float\"")]
            public static readonly dom.Literals.Types.OES_texture_half_float OES_texture_half_float;
            [Template("<self>\"OES_texture_float_linear\"")]
            public static readonly dom.Literals.Types.OES_texture_float_linear OES_texture_float_linear;
            [Template("<self>\"OES_texture_float\"")]
            public static readonly dom.Literals.Types.OES_texture_float OES_texture_float;
            [Template("<self>\"OES_standard_derivatives\"")]
            public static readonly dom.Literals.Types.OES_standard_derivatives OES_standard_derivatives;
            [Template("<self>\"OES_element_index_uint\"")]
            public static readonly dom.Literals.Types.OES_element_index_uint OES_element_index_uint;
            [Template("<self>\"ANGLE_instanced_arrays\"")]
            public static readonly dom.Literals.Types.ANGLE_instanced_arrays ANGLE_instanced_arrays;

            public static class Types
            {
                [Name("System.String")]
                public class drag : LiteralType<string>
                {
                    private extern drag();
                }

                [Name("System.String")]
                public class dragend : LiteralType<string>
                {
                    private extern dragend();
                }

                [Name("System.String")]
                public class dragenter : LiteralType<string>
                {
                    private extern dragenter();
                }

                [Name("System.String")]
                public class dragexit : LiteralType<string>
                {
                    private extern dragexit();
                }

                [Name("System.String")]
                public class dragleave : LiteralType<string>
                {
                    private extern dragleave();
                }

                [Name("System.String")]
                public class dragover : LiteralType<string>
                {
                    private extern dragover();
                }

                [Name("System.String")]
                public class dragstart : LiteralType<string>
                {
                    private extern dragstart();
                }

                [Name("System.String")]
                public class drop : LiteralType<string>
                {
                    private extern drop();
                }

                [Name("System.String")]
                public class auto : LiteralType<string>
                {
                    private extern auto();
                }

                [Name("System.String")]
                public class instant : LiteralType<string>
                {
                    private extern instant();
                }

                [Name("System.String")]
                public class smooth : LiteralType<string>
                {
                    private extern smooth();
                }

                [Name("System.String")]
                public class start : LiteralType<string>
                {
                    private extern start();
                }

                [Name("System.String")]
                public class center : LiteralType<string>
                {
                    private extern center();
                }

                [Name("System.String")]
                public class end : LiteralType<string>
                {
                    private extern end();
                }

                [Name("System.String")]
                public class nearest : LiteralType<string>
                {
                    private extern nearest();
                }

                [Name("System.String")]
                public class manual : LiteralType<string>
                {
                    private extern manual();
                }

                [Name("System.String")]
                public class beforebegin : LiteralType<string>
                {
                    private extern beforebegin();
                }

                [Name("System.String")]
                public class afterbegin : LiteralType<string>
                {
                    private extern afterbegin();
                }

                [Name("System.String")]
                public class beforeend : LiteralType<string>
                {
                    private extern beforeend();
                }

                [Name("System.String")]
                public class afterend : LiteralType<string>
                {
                    private extern afterend();
                }

                [Name("System.String")]
                public class any : LiteralType<string>
                {
                    private extern any();
                }

                [Name("System.String")]
                public class natural : LiteralType<string>
                {
                    private extern natural();
                }

                [Name("System.String")]
                public class portrait : LiteralType<string>
                {
                    private extern portrait();
                }

                [Name("System.String")]
                public class landscape : LiteralType<string>
                {
                    private extern landscape();
                }

                [Name("System.String")]
                public class portrait_primary : LiteralType<string>
                {
                    private extern portrait_primary();
                }

                [Name("System.String")]
                public class portrait_secondary : LiteralType<string>
                {
                    private extern portrait_secondary();
                }

                [Name("System.String")]
                public class landscape_primary : LiteralType<string>
                {
                    private extern landscape_primary();
                }

                [Name("System.String")]
                public class landscape_secondary : LiteralType<string>
                {
                    private extern landscape_secondary();
                }

                [Name("System.String")]
                public class attributes : LiteralType<string>
                {
                    private extern attributes();
                }

                [Name("System.String")]
                public class characterData : LiteralType<string>
                {
                    private extern characterData();
                }

                [Name("System.String")]
                public class childList : LiteralType<string>
                {
                    private extern childList();
                }

                [Name("System.String")]
                public class window : LiteralType<string>
                {
                    private extern window();
                }

                [Name("System.String")]
                public class worker : LiteralType<string>
                {
                    private extern worker();
                }

                [Name("System.String")]
                public class sharedworker : LiteralType<string>
                {
                    private extern sharedworker();
                }

                [Name("System.String")]
                public class all : LiteralType<string>
                {
                    private extern all();
                }

                [Name("System.String")]
                public class segments : LiteralType<string>
                {
                    private extern segments();
                }

                [Name("System.String")]
                public class sequence : LiteralType<string>
                {
                    private extern sequence();
                }

                [Name("System.String")]
                public class balanced : LiteralType<string>
                {
                    private extern balanced();
                }

                [Name("System.String")]
                public class interactive : LiteralType<string>
                {
                    private extern interactive();
                }

                [Name("System.String")]
                public class playback : LiteralType<string>
                {
                    private extern playback();
                }

                [Name("System.String")]
                public class suspended : LiteralType<string>
                {
                    private extern suspended();
                }

                [Name("System.String")]
                public class running : LiteralType<string>
                {
                    private extern running();
                }

                [Name("System.String")]
                public class closed : LiteralType<string>
                {
                    private extern closed();
                }

                [Name("System.String")]
                public class blob : LiteralType<string>
                {
                    private extern blob();
                }

                [Name("System.String")]
                public class arraybuffer : LiteralType<string>
                {
                    private extern arraybuffer();
                }

                [Name("System.String")]
                public class lowpass : LiteralType<string>
                {
                    private extern lowpass();
                }

                [Name("System.String")]
                public class highpass : LiteralType<string>
                {
                    private extern highpass();
                }

                [Name("System.String")]
                public class bandpass : LiteralType<string>
                {
                    private extern bandpass();
                }

                [Name("System.String")]
                public class lowshelf : LiteralType<string>
                {
                    private extern lowshelf();
                }

                [Name("System.String")]
                public class highshelf : LiteralType<string>
                {
                    private extern highshelf();
                }

                [Name("System.String")]
                public class peaking : LiteralType<string>
                {
                    private extern peaking();
                }

                [Name("System.String")]
                public class notch : LiteralType<string>
                {
                    private extern notch();
                }

                [Name("System.String")]
                public class allpass : LiteralType<string>
                {
                    private extern allpass();
                }

                [Name("System.String")]
                public class _ : LiteralType<string>
                {
                    private extern _();
                }

                [Name("System.String")]
                public class maybe : LiteralType<string>
                {
                    private extern maybe();
                }

                [Name("System.String")]
                public class probably : LiteralType<string>
                {
                    private extern probably();
                }

                [Name("System.String")]
                public class nonzero : LiteralType<string>
                {
                    private extern nonzero();
                }

                [Name("System.String")]
                public class evenodd : LiteralType<string>
                {
                    private extern evenodd();
                }

                [Name("System.String")]
                public class max : LiteralType<string>
                {
                    private extern max();
                }

                [Name("System.String")]
                public class clamped_max : LiteralType<string>
                {
                    private extern clamped_max();
                }

                [Name("System.String")]
                public class @explicit : LiteralType<string>
                {
                    private extern @explicit();
                }

                [Name("System.String")]
                public class speakers : LiteralType<string>
                {
                    private extern speakers();
                }

                [Name("System.String")]
                public class discrete : LiteralType<string>
                {
                    private extern discrete();
                }

                [Name("System.String")]
                public class monitor : LiteralType<string>
                {
                    private extern monitor();
                }

                [Name("System.String")]
                public class application : LiteralType<string>
                {
                    private extern application();
                }

                [Name("System.String")]
                public class browser : LiteralType<string>
                {
                    private extern browser();
                }

                [Name("System.String")]
                public class linear : LiteralType<string>
                {
                    private extern linear();
                }

                [Name("System.String")]
                public class inverse : LiteralType<string>
                {
                    private extern inverse();
                }

                [Name("System.String")]
                public class exponential : LiteralType<string>
                {
                    private extern exponential();
                }

                [Name("System.String")]
                public class network : LiteralType<string>
                {
                    private extern network();
                }

                [Name("System.String")]
                public class decode : LiteralType<string>
                {
                    private extern decode();
                }

                [Name("System.String")]
                public class character : LiteralType<string>
                {
                    private extern character();
                }

                [Name("System.String")]
                public class word : LiteralType<string>
                {
                    private extern word();
                }

                [Name("System.String")]
                public class sentence : LiteralType<string>
                {
                    private extern sentence();
                }

                [Name("System.String")]
                public class textedit : LiteralType<string>
                {
                    private extern textedit();
                }

                [Name("System.String")]
                public class left : LiteralType<string>
                {
                    private extern left();
                }

                [Name("System.String")]
                public class right : LiteralType<string>
                {
                    private extern right();
                }

                [Name("System.String")]
                public class vibration : LiteralType<string>
                {
                    private extern vibration();
                }

                [Name("System.String")]
                public class mouse : LiteralType<string>
                {
                    private extern mouse();
                }

                [Name("System.String")]
                public class keyboard : LiteralType<string>
                {
                    private extern keyboard();
                }

                [Name("System.String")]
                public class gamepad : LiteralType<string>
                {
                    private extern gamepad();
                }

                [Name("System.String")]
                public class standard : LiteralType<string>
                {
                    private extern standard();
                }

                [Name("System.String")]
                public class next : LiteralType<string>
                {
                    private extern next();
                }

                [Name("System.String")]
                public class nextunique : LiteralType<string>
                {
                    private extern nextunique();
                }

                [Name("System.String")]
                public class prev : LiteralType<string>
                {
                    private extern prev();
                }

                [Name("System.String")]
                public class prevunique : LiteralType<string>
                {
                    private extern prevunique();
                }

                [Name("System.String")]
                public class pending : LiteralType<string>
                {
                    private extern pending();
                }

                [Name("System.String")]
                public class done : LiteralType<string>
                {
                    private extern done();
                }

                [Name("System.String")]
                public class @readonly : LiteralType<string>
                {
                    private extern @readonly();
                }

                [Name("System.String")]
                public class readwrite : LiteralType<string>
                {
                    private extern readwrite();
                }

                [Name("System.String")]
                public class versionchange : LiteralType<string>
                {
                    private extern versionchange();
                }

                [Name("System.String")]
                public class raw : LiteralType<string>
                {
                    private extern raw();
                }

                [Name("System.String")]
                public class spki : LiteralType<string>
                {
                    private extern spki();
                }

                [Name("System.String")]
                public class pkcs8 : LiteralType<string>
                {
                    private extern pkcs8();
                }

                [Name("System.String")]
                public class jwk : LiteralType<string>
                {
                    private extern jwk();
                }

                [Name("System.String")]
                public class @public : LiteralType<string>
                {
                    private extern @public();
                }

                [Name("System.String")]
                public class @private : LiteralType<string>
                {
                    private extern @private();
                }

                [Name("System.String")]
                public class secret : LiteralType<string>
                {
                    private extern secret();
                }

                [Name("System.String")]
                public class encrypt : LiteralType<string>
                {
                    private extern encrypt();
                }

                [Name("System.String")]
                public class decrypt : LiteralType<string>
                {
                    private extern decrypt();
                }

                [Name("System.String")]
                public class sign : LiteralType<string>
                {
                    private extern sign();
                }

                [Name("System.String")]
                public class verify : LiteralType<string>
                {
                    private extern verify();
                }

                [Name("System.String")]
                public class deriveKey : LiteralType<string>
                {
                    private extern deriveKey();
                }

                [Name("System.String")]
                public class deriveBits : LiteralType<string>
                {
                    private extern deriveBits();
                }

                [Name("System.String")]
                public class wrapKey : LiteralType<string>
                {
                    private extern wrapKey();
                }

                [Name("System.String")]
                public class unwrapKey : LiteralType<string>
                {
                    private extern unwrapKey();
                }

                [Name("System.String")]
                public class inactive : LiteralType<string>
                {
                    private extern inactive();
                }

                [Name("System.String")]
                public class active : LiteralType<string>
                {
                    private extern active();
                }

                [Name("System.String")]
                public class disambiguation : LiteralType<string>
                {
                    private extern disambiguation();
                }

                [Name("System.String")]
                public class FIDO_2_0 : LiteralType<string>
                {
                    private extern FIDO_2_0();
                }

                [Name("System.String")]
                public class os : LiteralType<string>
                {
                    private extern os();
                }

                [Name("System.String")]
                public class stun : LiteralType<string>
                {
                    private extern stun();
                }

                [Name("System.String")]
                public class turn : LiteralType<string>
                {
                    private extern turn();
                }

                [Name("System.String")]
                public class peer_derived : LiteralType<string>
                {
                    private extern peer_derived();
                }

                [Name("System.String")]
                public class failed : LiteralType<string>
                {
                    private extern failed();
                }

                [Name("System.String")]
                public class direct : LiteralType<string>
                {
                    private extern direct();
                }

                [Name("System.String")]
                public class relay : LiteralType<string>
                {
                    private extern relay();
                }

                [Name("System.String")]
                public class description : LiteralType<string>
                {
                    private extern description();
                }

                [Name("System.String")]
                public class localclientevent : LiteralType<string>
                {
                    private extern localclientevent();
                }

                [Name("System.String")]
                public class inbound_network : LiteralType<string>
                {
                    private extern inbound_network();
                }

                [Name("System.String")]
                public class outbound_network : LiteralType<string>
                {
                    private extern outbound_network();
                }

                [Name("System.String")]
                public class inbound_payload : LiteralType<string>
                {
                    private extern inbound_payload();
                }

                [Name("System.String")]
                public class outbound_payload : LiteralType<string>
                {
                    private extern outbound_payload();
                }

                [Name("System.String")]
                public class transportdiagnostics : LiteralType<string>
                {
                    private extern transportdiagnostics();
                }

                [Name("System.String")]
                public class Embedded : LiteralType<string>
                {
                    private extern Embedded();
                }

                [Name("System.String")]
                public class USB : LiteralType<string>
                {
                    private extern USB();
                }

                [Name("System.String")]
                public class NFC : LiteralType<string>
                {
                    private extern NFC();
                }

                [Name("System.String")]
                public class BT : LiteralType<string>
                {
                    private extern BT();
                }

                [Name("System.String")]
                public class unknown : LiteralType<string>
                {
                    private extern unknown();
                }

                [Name("System.String")]
                public class defer : LiteralType<string>
                {
                    private extern defer();
                }

                [Name("System.String")]
                public class allow : LiteralType<string>
                {
                    private extern allow();
                }

                [Name("System.String")]
                public class deny : LiteralType<string>
                {
                    private extern deny();
                }

                [Name("System.String")]
                public class geolocation : LiteralType<string>
                {
                    private extern geolocation();
                }

                [Name("System.String")]
                public class unlimitedIndexedDBQuota : LiteralType<string>
                {
                    private extern unlimitedIndexedDBQuota();
                }

                [Name("System.String")]
                public class media : LiteralType<string>
                {
                    private extern media();
                }

                [Name("System.String")]
                public class pointerlock : LiteralType<string>
                {
                    private extern pointerlock();
                }

                [Name("System.String")]
                public class webnotifications : LiteralType<string>
                {
                    private extern webnotifications();
                }

                [Name("System.String")]
                public class audioinput : LiteralType<string>
                {
                    private extern audioinput();
                }

                [Name("System.String")]
                public class audiooutput : LiteralType<string>
                {
                    private extern audiooutput();
                }

                [Name("System.String")]
                public class videoinput : LiteralType<string>
                {
                    private extern videoinput();
                }

                [Name("System.String")]
                public class license_request : LiteralType<string>
                {
                    private extern license_request();
                }

                [Name("System.String")]
                public class license_renewal : LiteralType<string>
                {
                    private extern license_renewal();
                }

                [Name("System.String")]
                public class license_release : LiteralType<string>
                {
                    private extern license_release();
                }

                [Name("System.String")]
                public class individualization_request : LiteralType<string>
                {
                    private extern individualization_request();
                }

                [Name("System.String")]
                public class temporary : LiteralType<string>
                {
                    private extern temporary();
                }

                [Name("System.String")]
                public class persistent_license : LiteralType<string>
                {
                    private extern persistent_license();
                }

                [Name("System.String")]
                public class persistent_release_message : LiteralType<string>
                {
                    private extern persistent_release_message();
                }

                [Name("System.String")]
                public class usable : LiteralType<string>
                {
                    private extern usable();
                }

                [Name("System.String")]
                public class expired : LiteralType<string>
                {
                    private extern expired();
                }

                [Name("System.String")]
                public class output_downscaled : LiteralType<string>
                {
                    private extern output_downscaled();
                }

                [Name("System.String")]
                public class output_not_allowed : LiteralType<string>
                {
                    private extern output_not_allowed();
                }

                [Name("System.String")]
                public class status_pending : LiteralType<string>
                {
                    private extern status_pending();
                }

                [Name("System.String")]
                public class internal_error : LiteralType<string>
                {
                    private extern internal_error();
                }

                [Name("System.String")]
                public class required : LiteralType<string>
                {
                    private extern required();
                }

                [Name("System.String")]
                public class optional : LiteralType<string>
                {
                    private extern optional();
                }

                [Name("System.String")]
                public class not_allowed : LiteralType<string>
                {
                    private extern not_allowed();
                }

                [Name("System.String")]
                public class live : LiteralType<string>
                {
                    private extern live();
                }

                [Name("System.String")]
                public class ended : LiteralType<string>
                {
                    private extern ended();
                }

                [Name("System.String")]
                public class up : LiteralType<string>
                {
                    private extern up();
                }

                [Name("System.String")]
                public class down : LiteralType<string>
                {
                    private extern down();
                }

                [Name("System.String")]
                public class navigate : LiteralType<string>
                {
                    private extern navigate();
                }

                [Name("System.String")]
                public class reload : LiteralType<string>
                {
                    private extern reload();
                }

                [Name("System.String")]
                public class back_forward : LiteralType<string>
                {
                    private extern back_forward();
                }

                [Name("System.String")]
                public class prerender : LiteralType<string>
                {
                    private extern prerender();
                }

                [Name("System.String")]
                public class ltr : LiteralType<string>
                {
                    private extern ltr();
                }

                [Name("System.String")]
                public class rtl : LiteralType<string>
                {
                    private extern rtl();
                }

                [Name("System.String")]
                public class @default : LiteralType<string>
                {
                    private extern @default();
                }

                [Name("System.String")]
                public class denied : LiteralType<string>
                {
                    private extern denied();
                }

                [Name("System.String")]
                public class granted : LiteralType<string>
                {
                    private extern granted();
                }

                [Name("System.String")]
                public class sine : LiteralType<string>
                {
                    private extern sine();
                }

                [Name("System.String")]
                public class square : LiteralType<string>
                {
                    private extern square();
                }

                [Name("System.String")]
                public class sawtooth : LiteralType<string>
                {
                    private extern sawtooth();
                }

                [Name("System.String")]
                public class triangle : LiteralType<string>
                {
                    private extern triangle();
                }

                [Name("System.String")]
                public class custom : LiteralType<string>
                {
                    private extern custom();
                }

                [Name("System.String")]
                public class none : LiteralType<string>
                {
                    private extern none();
                }

                [Name("System.String")]
                public class _2x : LiteralType<string>
                {
                    private extern _2x();
                }

                [Name("System.String")]
                public class _4x : LiteralType<string>
                {
                    private extern _4x();
                }

                [Name("System.String")]
                public class equalpower : LiteralType<string>
                {
                    private extern equalpower();
                }

                [Name("System.String")]
                public class HRTF : LiteralType<string>
                {
                    private extern HRTF();
                }

                [Name("System.String")]
                public class success : LiteralType<string>
                {
                    private extern success();
                }

                [Name("System.String")]
                public class fail : LiteralType<string>
                {
                    private extern fail();
                }

                [Name("System.String")]
                public class shipping : LiteralType<string>
                {
                    private extern shipping();
                }

                [Name("System.String")]
                public class delivery : LiteralType<string>
                {
                    private extern delivery();
                }

                [Name("System.String")]
                public class pickup : LiteralType<string>
                {
                    private extern pickup();
                }

                [Name("System.String")]
                public class p256dh : LiteralType<string>
                {
                    private extern p256dh();
                }

                [Name("System.String")]
                public class auth : LiteralType<string>
                {
                    private extern auth();
                }

                [Name("System.String")]
                public class prompt : LiteralType<string>
                {
                    private extern prompt();
                }

                [Name("System.String")]
                public class max_compat : LiteralType<string>
                {
                    private extern max_compat();
                }

                [Name("System.String")]
                public class max_bundle : LiteralType<string>
                {
                    private extern max_bundle();
                }

                [Name("System.String")]
                public class maintain_framerate : LiteralType<string>
                {
                    private extern maintain_framerate();
                }

                [Name("System.String")]
                public class maintain_resolution : LiteralType<string>
                {
                    private extern maintain_resolution();
                }

                [Name("System.String")]
                public class client : LiteralType<string>
                {
                    private extern client();
                }

                [Name("System.String")]
                public class server : LiteralType<string>
                {
                    private extern server();
                }

                [Name("System.String")]
                public class @new : LiteralType<string>
                {
                    private extern @new();
                }

                [Name("System.String")]
                public class connecting : LiteralType<string>
                {
                    private extern connecting();
                }

                [Name("System.String")]
                public class connected : LiteralType<string>
                {
                    private extern connected();
                }

                [Name("System.String")]
                public class host : LiteralType<string>
                {
                    private extern host();
                }

                [Name("System.String")]
                public class srflx : LiteralType<string>
                {
                    private extern srflx();
                }

                [Name("System.String")]
                public class prflx : LiteralType<string>
                {
                    private extern prflx();
                }

                [Name("System.String")]
                public class RTP : LiteralType<string>
                {
                    private extern RTP();
                }

                [Name("System.String")]
                public class RTCP : LiteralType<string>
                {
                    private extern RTCP();
                }

                [Name("System.String")]
                public class checking : LiteralType<string>
                {
                    private extern checking();
                }

                [Name("System.String")]
                public class completed : LiteralType<string>
                {
                    private extern completed();
                }

                [Name("System.String")]
                public class disconnected : LiteralType<string>
                {
                    private extern disconnected();
                }

                [Name("System.String")]
                public class nohost : LiteralType<string>
                {
                    private extern nohost();
                }

                [Name("System.String")]
                public class gathering : LiteralType<string>
                {
                    private extern gathering();
                }

                [Name("System.String")]
                public class complete : LiteralType<string>
                {
                    private extern complete();
                }

                [Name("System.String")]
                public class udp : LiteralType<string>
                {
                    private extern udp();
                }

                [Name("System.String")]
                public class tcp : LiteralType<string>
                {
                    private extern tcp();
                }

                [Name("System.String")]
                public class controlling : LiteralType<string>
                {
                    private extern controlling();
                }

                [Name("System.String")]
                public class controlled : LiteralType<string>
                {
                    private extern controlled();
                }

                [Name("System.String")]
                public class passive : LiteralType<string>
                {
                    private extern passive();
                }

                [Name("System.String")]
                public class so : LiteralType<string>
                {
                    private extern so();
                }

                [Name("System.String")]
                public class offer : LiteralType<string>
                {
                    private extern offer();
                }

                [Name("System.String")]
                public class pranswer : LiteralType<string>
                {
                    private extern pranswer();
                }

                [Name("System.String")]
                public class answer : LiteralType<string>
                {
                    private extern answer();
                }

                [Name("System.String")]
                public class stable : LiteralType<string>
                {
                    private extern stable();
                }

                [Name("System.String")]
                public class have_local_offer : LiteralType<string>
                {
                    private extern have_local_offer();
                }

                [Name("System.String")]
                public class have_remote_offer : LiteralType<string>
                {
                    private extern have_remote_offer();
                }

                [Name("System.String")]
                public class have_local_pranswer : LiteralType<string>
                {
                    private extern have_local_pranswer();
                }

                [Name("System.String")]
                public class have_remote_pranswer : LiteralType<string>
                {
                    private extern have_remote_pranswer();
                }

                [Name("System.String")]
                public class frozen : LiteralType<string>
                {
                    private extern frozen();
                }

                [Name("System.String")]
                public class waiting : LiteralType<string>
                {
                    private extern waiting();
                }

                [Name("System.String")]
                public class inprogress : LiteralType<string>
                {
                    private extern inprogress();
                }

                [Name("System.String")]
                public class succeeded : LiteralType<string>
                {
                    private extern succeeded();
                }

                [Name("System.String")]
                public class cancelled : LiteralType<string>
                {
                    private extern cancelled();
                }

                [Name("System.String")]
                public class serverreflexive : LiteralType<string>
                {
                    private extern serverreflexive();
                }

                [Name("System.String")]
                public class peerreflexive : LiteralType<string>
                {
                    private extern peerreflexive();
                }

                [Name("System.String")]
                public class relayed : LiteralType<string>
                {
                    private extern relayed();
                }

                [Name("System.String")]
                public class inboundrtp : LiteralType<string>
                {
                    private extern inboundrtp();
                }

                [Name("System.String")]
                public class outboundrtp : LiteralType<string>
                {
                    private extern outboundrtp();
                }

                [Name("System.String")]
                public class session : LiteralType<string>
                {
                    private extern session();
                }

                [Name("System.String")]
                public class datachannel : LiteralType<string>
                {
                    private extern datachannel();
                }

                [Name("System.String")]
                public class track : LiteralType<string>
                {
                    private extern track();
                }

                [Name("System.String")]
                public class transport : LiteralType<string>
                {
                    private extern transport();
                }

                [Name("System.String")]
                public class candidatepair : LiteralType<string>
                {
                    private extern candidatepair();
                }

                [Name("System.String")]
                public class localcandidate : LiteralType<string>
                {
                    private extern localcandidate();
                }

                [Name("System.String")]
                public class remotecandidate : LiteralType<string>
                {
                    private extern remotecandidate();
                }

                [Name("System.String")]
                public class open : LiteralType<string>
                {
                    private extern open();
                }

                [Name("System.String")]
                public class no_referrer : LiteralType<string>
                {
                    private extern no_referrer();
                }

                [Name("System.String")]
                public class no_referrer_when_downgrade : LiteralType<string>
                {
                    private extern no_referrer_when_downgrade();
                }

                [Name("System.String")]
                public class origin_only : LiteralType<string>
                {
                    private extern origin_only();
                }

                [Name("System.String")]
                public class origin_when_cross_origin : LiteralType<string>
                {
                    private extern origin_when_cross_origin();
                }

                [Name("System.String")]
                public class unsafe_url : LiteralType<string>
                {
                    private extern unsafe_url();
                }

                [Name("System.String")]
                public class no_store : LiteralType<string>
                {
                    private extern no_store();
                }

                [Name("System.String")]
                public class no_cache : LiteralType<string>
                {
                    private extern no_cache();
                }

                [Name("System.String")]
                public class force_cache : LiteralType<string>
                {
                    private extern force_cache();
                }

                [Name("System.String")]
                public class omit : LiteralType<string>
                {
                    private extern omit();
                }

                [Name("System.String")]
                public class same_origin : LiteralType<string>
                {
                    private extern same_origin();
                }

                [Name("System.String")]
                public class include : LiteralType<string>
                {
                    private extern include();
                }

                [Name("System.String")]
                public class document : LiteralType<string>
                {
                    private extern document();
                }

                [Name("System.String")]
                public class subresource : LiteralType<string>
                {
                    private extern subresource();
                }

                [Name("System.String")]
                public class no_cors : LiteralType<string>
                {
                    private extern no_cors();
                }

                [Name("System.String")]
                public class cors : LiteralType<string>
                {
                    private extern cors();
                }

                [Name("System.String")]
                public class follow : LiteralType<string>
                {
                    private extern follow();
                }

                [Name("System.String")]
                public class error : LiteralType<string>
                {
                    private extern error();
                }

                [Name("System.String")]
                public class audio : LiteralType<string>
                {
                    private extern audio();
                }

                [Name("System.String")]
                public class font : LiteralType<string>
                {
                    private extern font();
                }

                [Name("System.String")]
                public class image : LiteralType<string>
                {
                    private extern image();
                }

                [Name("System.String")]
                public class script : LiteralType<string>
                {
                    private extern script();
                }

                [Name("System.String")]
                public class style : LiteralType<string>
                {
                    private extern style();
                }

                [Name("System.String")]
                public class video : LiteralType<string>
                {
                    private extern video();
                }

                [Name("System.String")]
                public class basic : LiteralType<string>
                {
                    private extern basic();
                }

                [Name("System.String")]
                public class opaque : LiteralType<string>
                {
                    private extern opaque();
                }

                [Name("System.String")]
                public class opaqueredirect : LiteralType<string>
                {
                    private extern opaqueredirect();
                }

                [Name("System.String")]
                public class ScopedCred : LiteralType<string>
                {
                    private extern ScopedCred();
                }

                [Name("System.String")]
                public class installing : LiteralType<string>
                {
                    private extern installing();
                }

                [Name("System.String")]
                public class installed : LiteralType<string>
                {
                    private extern installed();
                }

                [Name("System.String")]
                public class activating : LiteralType<string>
                {
                    private extern activating();
                }

                [Name("System.String")]
                public class activated : LiteralType<string>
                {
                    private extern activated();
                }

                [Name("System.String")]
                public class redundant : LiteralType<string>
                {
                    private extern redundant();
                }

                [Name("System.String")]
                public class subtitles : LiteralType<string>
                {
                    private extern subtitles();
                }

                [Name("System.String")]
                public class captions : LiteralType<string>
                {
                    private extern captions();
                }

                [Name("System.String")]
                public class descriptions : LiteralType<string>
                {
                    private extern descriptions();
                }

                [Name("System.String")]
                public class chapters : LiteralType<string>
                {
                    private extern chapters();
                }

                [Name("System.String")]
                public class metadata : LiteralType<string>
                {
                    private extern metadata();
                }

                [Name("System.String")]
                public class disabled : LiteralType<string>
                {
                    private extern disabled();
                }

                [Name("System.String")]
                public class hidden : LiteralType<string>
                {
                    private extern hidden();
                }

                [Name("System.String")]
                public class showing : LiteralType<string>
                {
                    private extern showing();
                }

                [Name("System.String")]
                public class usb : LiteralType<string>
                {
                    private extern usb();
                }

                [Name("System.String")]
                public class nfc : LiteralType<string>
                {
                    private extern nfc();
                }

                [Name("System.String")]
                public class ble : LiteralType<string>
                {
                    private extern ble();
                }

                [Name("System.String")]
                public class mounted : LiteralType<string>
                {
                    private extern mounted();
                }

                [Name("System.String")]
                public class navigation : LiteralType<string>
                {
                    private extern navigation();
                }

                [Name("System.String")]
                public class requested : LiteralType<string>
                {
                    private extern requested();
                }

                [Name("System.String")]
                public class unmounted : LiteralType<string>
                {
                    private extern unmounted();
                }

                [Name("System.String")]
                public class user : LiteralType<string>
                {
                    private extern user();
                }

                [Name("System.String")]
                public class environment : LiteralType<string>
                {
                    private extern environment();
                }

                [Name("System.String")]
                public class visible : LiteralType<string>
                {
                    private extern visible();
                }

                [Name("System.String")]
                public class unloaded : LiteralType<string>
                {
                    private extern unloaded();
                }

                [Name("System.String")]
                public class json : LiteralType<string>
                {
                    private extern json();
                }

                [Name("System.String")]
                public class text : LiteralType<string>
                {
                    private extern text();
                }

                [Name("System.String")]
                public class idle : LiteralType<string>
                {
                    private extern idle();
                }

                [Name("System.String")]
                public class paused : LiteralType<string>
                {
                    private extern paused();
                }

                [Name("System.String")]
                public class finished : LiteralType<string>
                {
                    private extern finished();
                }

                [Name("System.String")]
                public class normal : LiteralType<string>
                {
                    private extern normal();
                }

                [Name("System.String")]
                public class reverse : LiteralType<string>
                {
                    private extern reverse();
                }

                [Name("System.String")]
                public class alternate : LiteralType<string>
                {
                    private extern alternate();
                }

                [Name("System.String")]
                public class alternate_reverse : LiteralType<string>
                {
                    private extern alternate_reverse();
                }

                [Name("System.String")]
                public class forwards : LiteralType<string>
                {
                    private extern forwards();
                }

                [Name("System.String")]
                public class backwards : LiteralType<string>
                {
                    private extern backwards();
                }

                [Name("System.String")]
                public class both : LiteralType<string>
                {
                    private extern both();
                }

                [Name("System.String")]
                public class http_SlashSlashwww_w3_orgSlash1999Slashxhtml : LiteralType<string>
                {
                    private extern http_SlashSlashwww_w3_orgSlash1999Slashxhtml();
                }

                [Name("System.String")]
                public class http_SlashSlashwww_w3_orgSlash2000Slashsvg : LiteralType<string>
                {
                    private extern http_SlashSlashwww_w3_orgSlash2000Slashsvg();
                }

                [Name("System.String")]
                public class a : LiteralType<string>
                {
                    private extern a();
                }

                [Name("System.String")]
                public class circle : LiteralType<string>
                {
                    private extern circle();
                }

                [Name("System.String")]
                public class clipPath : LiteralType<string>
                {
                    private extern clipPath();
                }

                [Name("System.String")]
                public class componentTransferFunction : LiteralType<string>
                {
                    private extern componentTransferFunction();
                }

                [Name("System.String")]
                public class defs : LiteralType<string>
                {
                    private extern defs();
                }

                [Name("System.String")]
                public class desc : LiteralType<string>
                {
                    private extern desc();
                }

                [Name("System.String")]
                public class ellipse : LiteralType<string>
                {
                    private extern ellipse();
                }

                [Name("System.String")]
                public class feBlend : LiteralType<string>
                {
                    private extern feBlend();
                }

                [Name("System.String")]
                public class feColorMatrix : LiteralType<string>
                {
                    private extern feColorMatrix();
                }

                [Name("System.String")]
                public class feComponentTransfer : LiteralType<string>
                {
                    private extern feComponentTransfer();
                }

                [Name("System.String")]
                public class feComposite : LiteralType<string>
                {
                    private extern feComposite();
                }

                [Name("System.String")]
                public class feConvolveMatrix : LiteralType<string>
                {
                    private extern feConvolveMatrix();
                }

                [Name("System.String")]
                public class feDiffuseLighting : LiteralType<string>
                {
                    private extern feDiffuseLighting();
                }

                [Name("System.String")]
                public class feDisplacementMap : LiteralType<string>
                {
                    private extern feDisplacementMap();
                }

                [Name("System.String")]
                public class feDistantLight : LiteralType<string>
                {
                    private extern feDistantLight();
                }

                [Name("System.String")]
                public class feFlood : LiteralType<string>
                {
                    private extern feFlood();
                }

                [Name("System.String")]
                public class feFuncA : LiteralType<string>
                {
                    private extern feFuncA();
                }

                [Name("System.String")]
                public class feFuncB : LiteralType<string>
                {
                    private extern feFuncB();
                }

                [Name("System.String")]
                public class feFuncG : LiteralType<string>
                {
                    private extern feFuncG();
                }

                [Name("System.String")]
                public class feFuncR : LiteralType<string>
                {
                    private extern feFuncR();
                }

                [Name("System.String")]
                public class feGaussianBlur : LiteralType<string>
                {
                    private extern feGaussianBlur();
                }

                [Name("System.String")]
                public class feImage : LiteralType<string>
                {
                    private extern feImage();
                }

                [Name("System.String")]
                public class feMerge : LiteralType<string>
                {
                    private extern feMerge();
                }

                [Name("System.String")]
                public class feMergeNode : LiteralType<string>
                {
                    private extern feMergeNode();
                }

                [Name("System.String")]
                public class feMorphology : LiteralType<string>
                {
                    private extern feMorphology();
                }

                [Name("System.String")]
                public class feOffset : LiteralType<string>
                {
                    private extern feOffset();
                }

                [Name("System.String")]
                public class fePointLight : LiteralType<string>
                {
                    private extern fePointLight();
                }

                [Name("System.String")]
                public class feSpecularLighting : LiteralType<string>
                {
                    private extern feSpecularLighting();
                }

                [Name("System.String")]
                public class feSpotLight : LiteralType<string>
                {
                    private extern feSpotLight();
                }

                [Name("System.String")]
                public class feTile : LiteralType<string>
                {
                    private extern feTile();
                }

                [Name("System.String")]
                public class feTurbulence : LiteralType<string>
                {
                    private extern feTurbulence();
                }

                [Name("System.String")]
                public class filter : LiteralType<string>
                {
                    private extern filter();
                }

                [Name("System.String")]
                public class foreignObject : LiteralType<string>
                {
                    private extern foreignObject();
                }

                [Name("System.String")]
                public class g : LiteralType<string>
                {
                    private extern g();
                }

                [Name("System.String")]
                public class gradient : LiteralType<string>
                {
                    private extern gradient();
                }

                [Name("System.String")]
                public class line : LiteralType<string>
                {
                    private extern line();
                }

                [Name("System.String")]
                public class linearGradient : LiteralType<string>
                {
                    private extern linearGradient();
                }

                [Name("System.String")]
                public class marker : LiteralType<string>
                {
                    private extern marker();
                }

                [Name("System.String")]
                public class mask : LiteralType<string>
                {
                    private extern mask();
                }

                [Name("System.String")]
                public class path : LiteralType<string>
                {
                    private extern path();
                }

                [Name("System.String")]
                public class pattern : LiteralType<string>
                {
                    private extern pattern();
                }

                [Name("System.String")]
                public class polygon : LiteralType<string>
                {
                    private extern polygon();
                }

                [Name("System.String")]
                public class polyline : LiteralType<string>
                {
                    private extern polyline();
                }

                [Name("System.String")]
                public class radialGradient : LiteralType<string>
                {
                    private extern radialGradient();
                }

                [Name("System.String")]
                public class rect : LiteralType<string>
                {
                    private extern rect();
                }

                [Name("System.String")]
                public class svg : LiteralType<string>
                {
                    private extern svg();
                }

                [Name("System.String")]
                public class stop : LiteralType<string>
                {
                    private extern stop();
                }

                [Name("System.String")]
                public class @switch : LiteralType<string>
                {
                    private extern @switch();
                }

                [Name("System.String")]
                public class symbol : LiteralType<string>
                {
                    private extern symbol();
                }

                [Name("System.String")]
                public class tspan : LiteralType<string>
                {
                    private extern tspan();
                }

                [Name("System.String")]
                public class textContent : LiteralType<string>
                {
                    private extern textContent();
                }

                [Name("System.String")]
                public class textPath : LiteralType<string>
                {
                    private extern textPath();
                }

                [Name("System.String")]
                public class textPositioning : LiteralType<string>
                {
                    private extern textPositioning();
                }

                [Name("System.String")]
                public class title : LiteralType<string>
                {
                    private extern title();
                }

                [Name("System.String")]
                public class use : LiteralType<string>
                {
                    private extern use();
                }

                [Name("System.String")]
                public class view : LiteralType<string>
                {
                    private extern view();
                }

                [Name("System.String")]
                public class AnimationEvent : LiteralType<string>
                {
                    private extern AnimationEvent();
                }

                [Name("System.String")]
                public class AnimationPlaybackEvent : LiteralType<string>
                {
                    private extern AnimationPlaybackEvent();
                }

                [Name("System.String")]
                public class AudioProcessingEvent : LiteralType<string>
                {
                    private extern AudioProcessingEvent();
                }

                [Name("System.String")]
                public class BeforeUnloadEvent : LiteralType<string>
                {
                    private extern BeforeUnloadEvent();
                }

                [Name("System.String")]
                public class ClipboardEvent : LiteralType<string>
                {
                    private extern ClipboardEvent();
                }

                [Name("System.String")]
                public class CloseEvent : LiteralType<string>
                {
                    private extern CloseEvent();
                }

                [Name("System.String")]
                public class CompositionEvent : LiteralType<string>
                {
                    private extern CompositionEvent();
                }

                [Name("System.String")]
                public class CustomEvent : LiteralType<string>
                {
                    private extern CustomEvent();
                }

                [Name("System.String")]
                public class DeviceLightEvent : LiteralType<string>
                {
                    private extern DeviceLightEvent();
                }

                [Name("System.String")]
                public class DeviceMotionEvent : LiteralType<string>
                {
                    private extern DeviceMotionEvent();
                }

                [Name("System.String")]
                public class DeviceOrientationEvent : LiteralType<string>
                {
                    private extern DeviceOrientationEvent();
                }

                [Name("System.String")]
                public class DragEvent : LiteralType<string>
                {
                    private extern DragEvent();
                }

                [Name("System.String")]
                public class ErrorEvent : LiteralType<string>
                {
                    private extern ErrorEvent();
                }

                [Name("System.String")]
                public class Event : LiteralType<string>
                {
                    private extern Event();
                }

                [Name("System.String")]
                public class Events : LiteralType<string>
                {
                    private extern Events();
                }

                [Name("System.String")]
                public class FocusEvent : LiteralType<string>
                {
                    private extern FocusEvent();
                }

                [Name("System.String")]
                public class FocusNavigationEvent : LiteralType<string>
                {
                    private extern FocusNavigationEvent();
                }

                [Name("System.String")]
                public class GamepadEvent : LiteralType<string>
                {
                    private extern GamepadEvent();
                }

                [Name("System.String")]
                public class HashChangeEvent : LiteralType<string>
                {
                    private extern HashChangeEvent();
                }

                [Name("System.String")]
                public class IDBVersionChangeEvent : LiteralType<string>
                {
                    private extern IDBVersionChangeEvent();
                }

                [Name("System.String")]
                public class KeyboardEvent : LiteralType<string>
                {
                    private extern KeyboardEvent();
                }

                [Name("System.String")]
                public class ListeningStateChangedEvent : LiteralType<string>
                {
                    private extern ListeningStateChangedEvent();
                }

                [Name("System.String")]
                public class MSDCCEvent : LiteralType<string>
                {
                    private extern MSDCCEvent();
                }

                [Name("System.String")]
                public class MSDSHEvent : LiteralType<string>
                {
                    private extern MSDSHEvent();
                }

                [Name("System.String")]
                public class MSMediaKeyMessageEvent : LiteralType<string>
                {
                    private extern MSMediaKeyMessageEvent();
                }

                [Name("System.String")]
                public class MSMediaKeyNeededEvent : LiteralType<string>
                {
                    private extern MSMediaKeyNeededEvent();
                }

                [Name("System.String")]
                public class MediaEncryptedEvent : LiteralType<string>
                {
                    private extern MediaEncryptedEvent();
                }

                [Name("System.String")]
                public class MediaKeyMessageEvent : LiteralType<string>
                {
                    private extern MediaKeyMessageEvent();
                }

                [Name("System.String")]
                public class MediaStreamErrorEvent : LiteralType<string>
                {
                    private extern MediaStreamErrorEvent();
                }

                [Name("System.String")]
                public class MediaStreamEvent : LiteralType<string>
                {
                    private extern MediaStreamEvent();
                }

                [Name("System.String")]
                public class MediaStreamTrackEvent : LiteralType<string>
                {
                    private extern MediaStreamTrackEvent();
                }

                [Name("System.String")]
                public class MessageEvent : LiteralType<string>
                {
                    private extern MessageEvent();
                }

                [Name("System.String")]
                public class MouseEvent : LiteralType<string>
                {
                    private extern MouseEvent();
                }

                [Name("System.String")]
                public class MouseEvents : LiteralType<string>
                {
                    private extern MouseEvents();
                }

                [Name("System.String")]
                public class MutationEvent : LiteralType<string>
                {
                    private extern MutationEvent();
                }

                [Name("System.String")]
                public class MutationEvents : LiteralType<string>
                {
                    private extern MutationEvents();
                }

                [Name("System.String")]
                public class OfflineAudioCompletionEvent : LiteralType<string>
                {
                    private extern OfflineAudioCompletionEvent();
                }

                [Name("System.String")]
                public class OverflowEvent : LiteralType<string>
                {
                    private extern OverflowEvent();
                }

                [Name("System.String")]
                public class PageTransitionEvent : LiteralType<string>
                {
                    private extern PageTransitionEvent();
                }

                [Name("System.String")]
                public class PaymentRequestUpdateEvent : LiteralType<string>
                {
                    private extern PaymentRequestUpdateEvent();
                }

                [Name("System.String")]
                public class PermissionRequestedEvent : LiteralType<string>
                {
                    private extern PermissionRequestedEvent();
                }

                [Name("System.String")]
                public class PointerEvent : LiteralType<string>
                {
                    private extern PointerEvent();
                }

                [Name("System.String")]
                public class PopStateEvent : LiteralType<string>
                {
                    private extern PopStateEvent();
                }

                [Name("System.String")]
                public class ProgressEvent : LiteralType<string>
                {
                    private extern ProgressEvent();
                }

                [Name("System.String")]
                public class PromiseRejectionEvent : LiteralType<string>
                {
                    private extern PromiseRejectionEvent();
                }

                [Name("System.String")]
                public class RTCDTMFToneChangeEvent : LiteralType<string>
                {
                    private extern RTCDTMFToneChangeEvent();
                }

                [Name("System.String")]
                public class RTCDtlsTransportStateChangedEvent : LiteralType<string>
                {
                    private extern RTCDtlsTransportStateChangedEvent();
                }

                [Name("System.String")]
                public class RTCIceCandidatePairChangedEvent : LiteralType<string>
                {
                    private extern RTCIceCandidatePairChangedEvent();
                }

                [Name("System.String")]
                public class RTCIceGathererEvent : LiteralType<string>
                {
                    private extern RTCIceGathererEvent();
                }

                [Name("System.String")]
                public class RTCIceTransportStateChangedEvent : LiteralType<string>
                {
                    private extern RTCIceTransportStateChangedEvent();
                }

                [Name("System.String")]
                public class RTCPeerConnectionIceEvent : LiteralType<string>
                {
                    private extern RTCPeerConnectionIceEvent();
                }

                [Name("System.String")]
                public class RTCSsrcConflictEvent : LiteralType<string>
                {
                    private extern RTCSsrcConflictEvent();
                }

                [Name("System.String")]
                public class SVGZoomEvent : LiteralType<string>
                {
                    private extern SVGZoomEvent();
                }

                [Name("System.String")]
                public class SVGZoomEvents : LiteralType<string>
                {
                    private extern SVGZoomEvents();
                }

                [Name("System.String")]
                public class SecurityPolicyViolationEvent : LiteralType<string>
                {
                    private extern SecurityPolicyViolationEvent();
                }

                [Name("System.String")]
                public class ServiceWorkerMessageEvent : LiteralType<string>
                {
                    private extern ServiceWorkerMessageEvent();
                }

                [Name("System.String")]
                public class SpeechSynthesisEvent : LiteralType<string>
                {
                    private extern SpeechSynthesisEvent();
                }

                [Name("System.String")]
                public class StorageEvent : LiteralType<string>
                {
                    private extern StorageEvent();
                }

                [Name("System.String")]
                public class TextEvent : LiteralType<string>
                {
                    private extern TextEvent();
                }

                [Name("System.String")]
                public class TouchEvent : LiteralType<string>
                {
                    private extern TouchEvent();
                }

                [Name("System.String")]
                public class TrackEvent : LiteralType<string>
                {
                    private extern TrackEvent();
                }

                [Name("System.String")]
                public class TransitionEvent : LiteralType<string>
                {
                    private extern TransitionEvent();
                }

                [Name("System.String")]
                public class UIEvent : LiteralType<string>
                {
                    private extern UIEvent();
                }

                [Name("System.String")]
                public class UIEvents : LiteralType<string>
                {
                    private extern UIEvents();
                }

                [Name("System.String")]
                public class VRDisplayEvent : LiteralType<string>
                {
                    private extern VRDisplayEvent();
                }

                [Name("System.String")]
                public class VRDisplayEvent_ : LiteralType<string>
                {
                    private extern VRDisplayEvent_();
                }

                [Name("System.String")]
                public class WebGLContextEvent : LiteralType<string>
                {
                    private extern WebGLContextEvent();
                }

                [Name("System.String")]
                public class WheelEvent : LiteralType<string>
                {
                    private extern WheelEvent();
                }

                [Name("System.String")]
                public class _2d : LiteralType<string>
                {
                    private extern _2d();
                }

                [Name("System.String")]
                public class webgl : LiteralType<string>
                {
                    private extern webgl();
                }

                [Name("System.String")]
                public class experimental_webgl : LiteralType<string>
                {
                    private extern experimental_webgl();
                }

                [Name("System.String")]
                public class async : LiteralType<string>
                {
                    private extern async();
                }

                [Name("System.String")]
                public class sync : LiteralType<string>
                {
                    private extern sync();
                }

                [Name("System.String")]
                public class forward : LiteralType<string>
                {
                    private extern forward();
                }

                [Name("System.String")]
                public class backward : LiteralType<string>
                {
                    private extern backward();
                }

                [Name("System.String")]
                public class flipY : LiteralType<string>
                {
                    private extern flipY();
                }

                [Name("System.String")]
                public class premultiply : LiteralType<string>
                {
                    private extern premultiply();
                }

                [Name("System.String")]
                public class pixelated : LiteralType<string>
                {
                    private extern pixelated();
                }

                [Name("System.String")]
                public class low : LiteralType<string>
                {
                    private extern low();
                }

                [Name("System.String")]
                public class medium : LiteralType<string>
                {
                    private extern medium();
                }

                [Name("System.String")]
                public class high : LiteralType<string>
                {
                    private extern high();
                }

                [Name("System.String")]
                public class EXT_blend_minmax : LiteralType<string>
                {
                    private extern EXT_blend_minmax();
                }

                [Name("System.String")]
                public class EXT_texture_filter_anisotropic : LiteralType<string>
                {
                    private extern EXT_texture_filter_anisotropic();
                }

                [Name("System.String")]
                public class EXT_frag_depth : LiteralType<string>
                {
                    private extern EXT_frag_depth();
                }

                [Name("System.String")]
                public class EXT_shader_texture_lod : LiteralType<string>
                {
                    private extern EXT_shader_texture_lod();
                }

                [Name("System.String")]
                public class EXT_sRGB : LiteralType<string>
                {
                    private extern EXT_sRGB();
                }

                [Name("System.String")]
                public class OES_vertex_array_object : LiteralType<string>
                {
                    private extern OES_vertex_array_object();
                }

                [Name("System.String")]
                public class WEBGL_color_buffer_float : LiteralType<string>
                {
                    private extern WEBGL_color_buffer_float();
                }

                [Name("System.String")]
                public class WEBGL_compressed_texture_astc : LiteralType<string>
                {
                    private extern WEBGL_compressed_texture_astc();
                }

                [Name("System.String")]
                public class WEBGL_compressed_texture_s3tc_srgb : LiteralType<string>
                {
                    private extern WEBGL_compressed_texture_s3tc_srgb();
                }

                [Name("System.String")]
                public class WEBGL_debug_shaders : LiteralType<string>
                {
                    private extern WEBGL_debug_shaders();
                }

                [Name("System.String")]
                public class WEBGL_draw_buffers : LiteralType<string>
                {
                    private extern WEBGL_draw_buffers();
                }

                [Name("System.String")]
                public class WEBGL_lose_context : LiteralType<string>
                {
                    private extern WEBGL_lose_context();
                }

                [Name("System.String")]
                public class WEBGL_depth_texture : LiteralType<string>
                {
                    private extern WEBGL_depth_texture();
                }

                [Name("System.String")]
                public class WEBGL_debug_renderer_info : LiteralType<string>
                {
                    private extern WEBGL_debug_renderer_info();
                }

                [Name("System.String")]
                public class WEBGL_compressed_texture_s3tc : LiteralType<string>
                {
                    private extern WEBGL_compressed_texture_s3tc();
                }

                [Name("System.String")]
                public class OES_texture_half_float_linear : LiteralType<string>
                {
                    private extern OES_texture_half_float_linear();
                }

                [Name("System.String")]
                public class OES_texture_half_float : LiteralType<string>
                {
                    private extern OES_texture_half_float();
                }

                [Name("System.String")]
                public class OES_texture_float_linear : LiteralType<string>
                {
                    private extern OES_texture_float_linear();
                }

                [Name("System.String")]
                public class OES_texture_float : LiteralType<string>
                {
                    private extern OES_texture_float();
                }

                [Name("System.String")]
                public class OES_standard_derivatives : LiteralType<string>
                {
                    private extern OES_standard_derivatives();
                }

                [Name("System.String")]
                public class OES_element_index_uint : LiteralType<string>
                {
                    private extern OES_element_index_uint();
                }

                [Name("System.String")]
                public class ANGLE_instanced_arrays : LiteralType<string>
                {
                    private extern ANGLE_instanced_arrays();
                }
            }

            public static class Options
            {
                [Name("System.String")]
                public class type : LiteralType<string>
                {
                    [Template("<self>\"drag\"")]
                    public static readonly dom.Literals.Types.drag drag;
                    [Template("<self>\"dragend\"")]
                    public static readonly dom.Literals.Types.dragend dragend;
                    [Template("<self>\"dragenter\"")]
                    public static readonly dom.Literals.Types.dragenter dragenter;
                    [Template("<self>\"dragexit\"")]
                    public static readonly dom.Literals.Types.dragexit dragexit;
                    [Template("<self>\"dragleave\"")]
                    public static readonly dom.Literals.Types.dragleave dragleave;
                    [Template("<self>\"dragover\"")]
                    public static readonly dom.Literals.Types.dragover dragover;
                    [Template("<self>\"dragstart\"")]
                    public static readonly dom.Literals.Types.dragstart dragstart;
                    [Template("<self>\"drop\"")]
                    public static readonly dom.Literals.Types.drop drop;

                    private extern type();

                    public static extern implicit operator dom.Literals.Options.type(
                      dom.Literals.Types.drag value);

                    public static extern implicit operator dom.Literals.Options.type(
                      dom.Literals.Types.dragend value);

                    public static extern implicit operator dom.Literals.Options.type(
                      dom.Literals.Types.dragenter value);

                    public static extern implicit operator dom.Literals.Options.type(
                      dom.Literals.Types.dragexit value);

                    public static extern implicit operator dom.Literals.Options.type(
                      dom.Literals.Types.dragleave value);

                    public static extern implicit operator dom.Literals.Options.type(
                      dom.Literals.Types.dragover value);

                    public static extern implicit operator dom.Literals.Options.type(
                      dom.Literals.Types.dragstart value);

                    public static extern implicit operator dom.Literals.Options.type(
                      dom.Literals.Types.drop value);
                }

                [Name("System.String")]
                public class playState : LiteralType<string>
                {
                    [Template("<self>\"idle\"")]
                    public static readonly dom.Literals.Types.idle idle;
                    [Template("<self>\"running\"")]
                    public static readonly dom.Literals.Types.running running;
                    [Template("<self>\"paused\"")]
                    public static readonly dom.Literals.Types.paused paused;
                    [Template("<self>\"finished\"")]
                    public static readonly dom.Literals.Types.finished finished;

                    private extern playState();

                    public static extern implicit operator dom.Literals.Options.playState(
                      dom.Literals.Types.idle value);

                    public static extern implicit operator dom.Literals.Options.playState(
                      dom.Literals.Types.running value);

                    public static extern implicit operator dom.Literals.Options.playState(
                      dom.Literals.Types.paused value);

                    public static extern implicit operator dom.Literals.Options.playState(
                      dom.Literals.Types.finished value);
                }

                [Name("System.String")]
                public class direction : LiteralType<string>
                {
                    [Template("<self>\"normal\"")]
                    public static readonly dom.Literals.Types.normal normal;
                    [Template("<self>\"reverse\"")]
                    public static readonly dom.Literals.Types.reverse reverse;
                    [Template("<self>\"alternate\"")]
                    public static readonly dom.Literals.Types.alternate alternate;
                    [Template("<self>\"alternate-reverse\"")]
                    public static readonly dom.Literals.Types.alternate_reverse alternate_reverse;

                    private extern direction();

                    public static extern implicit operator dom.Literals.Options.direction(
                      dom.Literals.Types.normal value);

                    public static extern implicit operator dom.Literals.Options.direction(
                      dom.Literals.Types.reverse value);

                    public static extern implicit operator dom.Literals.Options.direction(
                      dom.Literals.Types.alternate value);

                    public static extern implicit operator dom.Literals.Options.direction(
                      dom.Literals.Types.alternate_reverse value);
                }

                [Name("System.String")]
                public class fill : LiteralType<string>
                {
                    [Template("<self>\"none\"")]
                    public static readonly dom.Literals.Types.none none;
                    [Template("<self>\"forwards\"")]
                    public static readonly dom.Literals.Types.forwards forwards;
                    [Template("<self>\"backwards\"")]
                    public static readonly dom.Literals.Types.backwards backwards;
                    [Template("<self>\"both\"")]
                    public static readonly dom.Literals.Types.both both;
                    [Template("<self>\"auto\"")]
                    public static readonly dom.Literals.Types.auto auto;

                    private extern fill();

                    public static extern implicit operator dom.Literals.Options.fill(
                      dom.Literals.Types.none value);

                    public static extern implicit operator dom.Literals.Options.fill(
                      dom.Literals.Types.forwards value);

                    public static extern implicit operator dom.Literals.Options.fill(
                      dom.Literals.Types.backwards value);

                    public static extern implicit operator dom.Literals.Options.fill(
                      dom.Literals.Types.both value);

                    public static extern implicit operator dom.Literals.Options.fill(
                      dom.Literals.Types.auto value);
                }

                [Name("System.String")]
                public class type2 : LiteralType<string>
                {
                    [Template("<self>\"drag\"")]
                    public static readonly dom.Literals.Types.drag drag;
                    [Template("<self>\"dragend\"")]
                    public static readonly dom.Literals.Types.dragend dragend;
                    [Template("<self>\"dragenter\"")]
                    public static readonly dom.Literals.Types.dragenter dragenter;
                    [Template("<self>\"dragexit\"")]
                    public static readonly dom.Literals.Types.dragexit dragexit;
                    [Template("<self>\"dragleave\"")]
                    public static readonly dom.Literals.Types.dragleave dragleave;
                    [Template("<self>\"dragover\"")]
                    public static readonly dom.Literals.Types.dragover dragover;
                    [Template("<self>\"dragstart\"")]
                    public static readonly dom.Literals.Types.dragstart dragstart;
                    [Template("<self>\"drop\"")]
                    public static readonly dom.Literals.Types.drop drop;

                    private extern type2();

                    public static extern implicit operator dom.Literals.Options.type2(
                      dom.Literals.Types.drag value);

                    public static extern implicit operator dom.Literals.Options.type2(
                      dom.Literals.Types.dragend value);

                    public static extern implicit operator dom.Literals.Options.type2(
                      dom.Literals.Types.dragenter value);

                    public static extern implicit operator dom.Literals.Options.type2(
                      dom.Literals.Types.dragexit value);

                    public static extern implicit operator dom.Literals.Options.type2(
                      dom.Literals.Types.dragleave value);

                    public static extern implicit operator dom.Literals.Options.type2(
                      dom.Literals.Types.dragover value);

                    public static extern implicit operator dom.Literals.Options.type2(
                      dom.Literals.Types.dragstart value);

                    public static extern implicit operator dom.Literals.Options.type2(
                      dom.Literals.Types.drop value);

                    public static extern implicit operator dom.Literals.Options.type2(
                      dom.Literals.Options.type value);
                }

                [Name("System.String")]
                public class contextId : LiteralType<string>
                {
                    [Template("<self>\"webgl\"")]
                    public static readonly dom.Literals.Types.webgl webgl;
                    [Template("<self>\"experimental-webgl\"")]
                    public static readonly dom.Literals.Types.experimental_webgl experimental_webgl;

                    private extern contextId();

                    public static extern implicit operator dom.Literals.Options.contextId(
                      dom.Literals.Types.webgl value);

                    public static extern implicit operator dom.Literals.Options.contextId(
                      dom.Literals.Types.experimental_webgl value);
                }

                [Name("System.String")]
                public class decoding : LiteralType<string>
                {
                    [Template("<self>\"async\"")]
                    public static readonly dom.Literals.Types.async async;
                    [Template("<self>\"sync\"")]
                    public static readonly dom.Literals.Types.sync sync;
                    [Template("<self>\"auto\"")]
                    public static readonly dom.Literals.Types.auto auto;

                    private extern decoding();

                    public static extern implicit operator dom.Literals.Options.decoding(
                      dom.Literals.Types.async value);

                    public static extern implicit operator dom.Literals.Options.decoding(
                      dom.Literals.Types.sync value);

                    public static extern implicit operator dom.Literals.Options.decoding(
                      dom.Literals.Types.auto value);
                }

                [Name("System.String")]
                public class direction2 : LiteralType<string>
                {
                    [Template("<self>\"forward\"")]
                    public static readonly dom.Literals.Types.forward forward;
                    [Template("<self>\"backward\"")]
                    public static readonly dom.Literals.Types.backward backward;
                    [Template("<self>\"none\"")]
                    public static readonly dom.Literals.Types.none none;

                    private extern direction2();

                    public static extern implicit operator dom.Literals.Options.direction2(
                      dom.Literals.Types.forward value);

                    public static extern implicit operator dom.Literals.Options.direction2(
                      dom.Literals.Types.backward value);

                    public static extern implicit operator dom.Literals.Options.direction2(
                      dom.Literals.Types.none value);
                }

                [Name("System.String")]
                public class direction3 : LiteralType<string>
                {
                    [Template("<self>\"forward\"")]
                    public static readonly dom.Literals.Types.forward forward;
                    [Template("<self>\"backward\"")]
                    public static readonly dom.Literals.Types.backward backward;
                    [Template("<self>\"none\"")]
                    public static readonly dom.Literals.Types.none none;

                    private extern direction3();

                    public static extern implicit operator dom.Literals.Options.direction3(
                      dom.Literals.Types.forward value);

                    public static extern implicit operator dom.Literals.Options.direction3(
                      dom.Literals.Types.backward value);

                    public static extern implicit operator dom.Literals.Options.direction3(
                      dom.Literals.Types.none value);

                    public static extern implicit operator dom.Literals.Options.direction3(
                      dom.Literals.Options.direction2 value);
                }

                [Name("System.String")]
                public class colorSpaceConversion : LiteralType<string>
                {
                    [Template("<self>\"none\"")]
                    public static readonly dom.Literals.Types.none none;
                    [Template("<self>\"default\"")]
                    public static readonly dom.Literals.Types.@default @default;

                    private extern colorSpaceConversion();

                    public static extern implicit operator dom.Literals.Options.colorSpaceConversion(
                      dom.Literals.Types.none value);

                    public static extern implicit operator dom.Literals.Options.colorSpaceConversion(
                      dom.Literals.Types.@default value);
                }

                [Name("System.String")]
                public class imageOrientation : LiteralType<string>
                {
                    [Template("<self>\"none\"")]
                    public static readonly dom.Literals.Types.none none;
                    [Template("<self>\"flipY\"")]
                    public static readonly dom.Literals.Types.flipY flipY;

                    private extern imageOrientation();

                    public static extern implicit operator dom.Literals.Options.imageOrientation(
                      dom.Literals.Types.none value);

                    public static extern implicit operator dom.Literals.Options.imageOrientation(
                      dom.Literals.Types.flipY value);
                }

                [Name("System.String")]
                public class premultiplyAlpha : LiteralType<string>
                {
                    [Template("<self>\"none\"")]
                    public static readonly dom.Literals.Types.none none;
                    [Template("<self>\"premultiply\"")]
                    public static readonly dom.Literals.Types.premultiply premultiply;
                    [Template("<self>\"default\"")]
                    public static readonly dom.Literals.Types.@default @default;

                    private extern premultiplyAlpha();

                    public static extern implicit operator dom.Literals.Options.premultiplyAlpha(
                      dom.Literals.Types.none value);

                    public static extern implicit operator dom.Literals.Options.premultiplyAlpha(
                      dom.Literals.Types.premultiply value);

                    public static extern implicit operator dom.Literals.Options.premultiplyAlpha(
                      dom.Literals.Types.@default value);

                    public static extern implicit operator dom.Literals.Options.premultiplyAlpha(
                      dom.Literals.Options.colorSpaceConversion value);
                }

                [Name("System.String")]
                public class resizeQuality : LiteralType<string>
                {
                    [Template("<self>\"pixelated\"")]
                    public static readonly dom.Literals.Types.pixelated pixelated;
                    [Template("<self>\"low\"")]
                    public static readonly dom.Literals.Types.low low;
                    [Template("<self>\"medium\"")]
                    public static readonly dom.Literals.Types.medium medium;
                    [Template("<self>\"high\"")]
                    public static readonly dom.Literals.Types.high high;

                    private extern resizeQuality();

                    public static extern implicit operator dom.Literals.Options.resizeQuality(
                      dom.Literals.Types.pixelated value);

                    public static extern implicit operator dom.Literals.Options.resizeQuality(
                      dom.Literals.Types.low value);

                    public static extern implicit operator dom.Literals.Options.resizeQuality(
                      dom.Literals.Types.medium value);

                    public static extern implicit operator dom.Literals.Options.resizeQuality(
                      dom.Literals.Types.high value);
                }

                [Name("System.String")]
                public class mode : LiteralType<string>
                {
                    [Template("<self>\"open\"")]
                    public static readonly dom.Literals.Types.open open;
                    [Template("<self>\"closed\"")]
                    public static readonly dom.Literals.Types.closed closed;

                    private extern mode();

                    public static extern implicit operator dom.Literals.Options.mode(
                      dom.Literals.Types.open value);

                    public static extern implicit operator dom.Literals.Options.mode(
                      dom.Literals.Types.closed value);
                }

                [Name("System.String")]
                public class format : LiteralType<string>
                {
                    [Template("<self>\"raw\"")]
                    public static readonly dom.Literals.Types.raw raw;
                    [Template("<self>\"pkcs8\"")]
                    public static readonly dom.Literals.Types.pkcs8 pkcs8;
                    [Template("<self>\"spki\"")]
                    public static readonly dom.Literals.Types.spki spki;

                    private extern format();

                    public static extern implicit operator dom.Literals.Options.format(
                      dom.Literals.Types.raw value);

                    public static extern implicit operator dom.Literals.Options.format(
                      dom.Literals.Types.pkcs8 value);

                    public static extern implicit operator dom.Literals.Options.format(
                      dom.Literals.Types.spki value);
                }
            }
        }
    }
}
