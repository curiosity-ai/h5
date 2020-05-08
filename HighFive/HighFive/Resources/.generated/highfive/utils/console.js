    H5.define("H5.Console", {
        statics: {
            fields: {
                BODY_WRAPPER_ID: null,
                CONSOLE_MESSAGES_ID: null,
                position: null,
                instance$1: null
            },
            props: {
                instance: {
                    get: function () {
                        if (H5.Console.instance$1 == null) {
                            H5.Console.instance$1 = new H5.Console();
                        }

                        return H5.Console.instance$1;
                    }
                }
            },
            ctors: {
                init: function () {
                    this.BODY_WRAPPER_ID = "h5-body-wrapper";
                    this.CONSOLE_MESSAGES_ID = "h5-console-messages";
                    this.position = "horizontal";
                }
            },
            methods: {
                initConsoleFunctions: function () {
                    var wl = System.Console.WriteLine;
                    var w = System.Console.Write;
                    var clr = System.Console.Clear;
                    var debug = System.Diagnostics.Debug.writeln;
                    var con = H5.global.console;

                    if (wl) {
                        System.Console.WriteLine = function (value) {
                            wl(value);
                            H5.Console.log(value, true);
                        }
                    }

                    if (w) {
                        System.Console.Write = function (value) {
                            w(value);
                            H5.Console.log(value, false);
                        }
                    }

                    if (clr) {
                        System.Console.Clear = function () {
                            clr();
                            H5.Console.clear();
                        }
                    }

                    if (debug) {
                        System.Diagnostics.Debug.writeln = function (value) {
                            debug(value);
                            H5.Console.debug(value);
                        }
                    }

                    if (con && con.error) {
                        var err = con.error;

                        con.error = function (msg) {
                            err.apply(con, arguments);
                            H5.Console.error(msg);
                        }
                    }

                    if (H5.isDefined(H5.global.window)) {
                        H5.global.window.addEventListener("error", function (e) {
                            H5.Console.error(System.Exception.create(e));
                        });
                    }
                },
                logBase: function (value, newLine, messageType) {
                    var $t;
                    if (newLine === void 0) { newLine = true; }
                    if (messageType === void 0) { messageType = 0; }
                    var self = H5.Console.instance;
                    var v = "";

                    if (value != null) {
                        var hasToString = value.ToString !== undefined;
                        v = (value.toString == { }.toString && !hasToString) ? JSON.stringify(value, null, 2) : hasToString ? value.ToString() : value.toString();
                    }

                    if (self.bufferedOutput != null) {
                        self.bufferedOutput = (self.bufferedOutput || "") + (v || "");

                        if (newLine) {
                            self.bufferedOutput = (self.bufferedOutput || "") + ("\n" || "");
                        }

                        return;
                    }

                    H5.Console.show();

                    if (self.isNewLine || self.currentMessageElement == null) {
                        var m = self.buildConsoleMessage(v, messageType);
                        self.consoleMessages.appendChild(m);
                        self.currentMessageElement = m;
                    } else {
                        var m1 = H5.unbox(self.currentMessageElement);
                        ($t = m1.lastChild).innerHTML = ($t.innerHTML || "") + (v || "");
                    }

                    self.isNewLine = newLine;
                },
                error: function (value) {
                    H5.Console.logBase(value, true, 2);
                },
                debug: function (value) {
                    H5.Console.logBase(value, true, 1);
                },
                log: function (value, newLine) {
                    if (newLine === void 0) { newLine = true; }
                    H5.Console.logBase(value, newLine);
                },
                clear: function () {
                    var self = H5.Console.instance$1;

                    if (self == null) {
                        return;
                    }

                    var m = self.consoleMessages;

                    if (m != null) {
                        while (m.firstChild != null) {
                            m.removeChild(m.firstChild);
                        }

                        self.currentMessageElement = null;
                    }

                    if (self.bufferedOutput != null) {
                        self.bufferedOutput = "";
                    }

                    self.isNewLine = false;
                },
                hide: function () {
                    if (H5.Console.instance$1 == null) {
                        return;
                    }

                    var self = H5.Console.instance;

                    if (self.hidden) {
                        return;
                    }

                    self.close();
                },
                show: function () {
                    var self = H5.Console.instance;

                    if (!self.hidden) {
                        return;
                    }

                    self.init(true);
                },
                toggle: function () {
                    if (H5.Console.instance.hidden) {
                        H5.Console.show();
                    } else {
                        H5.Console.hide();
                    }
                }
            }
        },
        fields: {
            svgNS: null,
            consoleHeight: null,
            consoleHeaderHeight: null,
            tooltip: null,
            consoleWrap: null,
            consoleMessages: null,
            highFiveIcon: null,
            highFiveIconPath: null,
            highFiveConsoleLabel: null,
            closeBtn: null,
            closeIcon: null,
            closeIconPath: null,
            consoleHeader: null,
            consoleBody: null,
            hidden: false,
            isNewLine: false,
            currentMessageElement: null,
            bufferedOutput: null
        },
        ctors: {
            init: function () {
                this.svgNS = "http://www.w3.org/2000/svg";
                this.consoleHeight = "300px";
                this.consoleHeaderHeight = "35px";
                this.hidden = true;
                this.isNewLine = false;
            },
            ctor: function () {
                this.$initialize();
                this.init();
            }
        },
        methods: {
            init: function (reinit) {
                if (reinit === void 0) { reinit = false; }
                this.hidden = false;

                var consoleWrapStyles = H5.fn.bind(this, $asm.$.H5.Console.f1)(new (System.Collections.Generic.Dictionary$2(System.String,System.String)).ctor());

                var consoleHeaderStyles = $asm.$.H5.Console.f2(new (System.Collections.Generic.Dictionary$2(System.String,System.String)).ctor());

                var consoleBodyStyles = $asm.$.H5.Console.f3(new (System.Collections.Generic.Dictionary$2(System.String,System.String)).ctor());

                this.highFiveIcon = this.highFiveIcon || document.createElementNS(this.svgNS, "svg");

                var items = H5.fn.bind(this, $asm.$.H5.Console.f4)(new (System.Collections.Generic.Dictionary$2(System.String,System.String)).ctor());

                this.setAttributes(this.highFiveIcon, items);

                this.highFiveIconPath = this.highFiveIconPath || document.createElementNS(this.svgNS, "path");

                var items2 = new (System.Collections.Generic.Dictionary$2(System.String,System.String)).ctor();
                items2.setItem("d", "M19 14.4h2.2V9.6L19 7.1v7.3zm4.3-2.5v2.5h2.2l-2.2-2.5zm-8.5 2.5H17V4.8l-2.2-2.5v12.1zM0 14.4h3l7.5-8.5v8.5h2.2V0L0 14.4z");
                items2.setItem("fill", "#555");

                this.setAttributes(this.highFiveIconPath, items2);

                this.highFiveConsoleLabel = this.highFiveConsoleLabel || document.createElement("span");
                this.highFiveConsoleLabel.innerHTML = "H5 Console";

                this.closeBtn = this.closeBtn || document.createElement("span");
                this.closeBtn.setAttribute("style", "position: relative;display: inline-block;float: right;cursor: pointer");

                this.closeIcon = this.closeIcon || document.createElementNS(this.svgNS, "svg");

                var items3 = H5.fn.bind(this, $asm.$.H5.Console.f5)(new (System.Collections.Generic.Dictionary$2(System.String,System.String)).ctor());

                this.setAttributes(this.closeIcon, items3);

                this.closeIconPath = this.closeIconPath || document.createElementNS(this.svgNS, "path");

                var items4 = $asm.$.H5.Console.f6(new (System.Collections.Generic.Dictionary$2(System.String,System.String)).ctor());

                this.setAttributes(this.closeIconPath, items4);

                this.tooltip = this.tooltip || document.createElement("div");
                this.tooltip.innerHTML = "Refresh page to open H5 Console";

                this.tooltip.setAttribute("style", "position: absolute;right: 30px;top: -6px;white-space: nowrap;padding: 7px;border-radius: 3px;background-color: rgba(0, 0, 0, 0.75);color: #eee;text-align: center;visibility: hidden;opacity: 0;-webkit-transition: all 0.25s ease-in-out;transition: all 0.25s ease-in-out;z-index: 1;");

                H5.Console.position = "horizontal";

                if (H5.referenceEquals(H5.Console.position, "horizontal")) {
                    this.wrapBodyContent();

                    consoleWrapStyles.setItem("right", "0");
                    consoleHeaderStyles.setItem("border-top", "1px solid #a3a3a3");
                    consoleBodyStyles.setItem("height", this.consoleHeight);
                } else if (H5.referenceEquals(H5.Console.position, "vertical")) {
                    var consoleWidth = "400px";
                    document.body.style.marginLeft = consoleWidth;

                    consoleWrapStyles.setItem("top", "0");
                    consoleWrapStyles.setItem("width", consoleWidth);
                    consoleWrapStyles.setItem("border-right", "1px solid #a3a3a3");
                    consoleBodyStyles.setItem("height", "100%");
                }

                this.consoleWrap = this.consoleWrap || document.createElement("div");
                this.consoleWrap.setAttribute("style", this.obj2Css(consoleWrapStyles));

                this.consoleHeader = this.consoleHeader || document.createElement("div");
                this.consoleHeader.setAttribute("style", this.obj2Css(consoleHeaderStyles));

                this.consoleBody = this.consoleBody || document.createElement("div");
                this.consoleBody.setAttribute("style", this.obj2Css(consoleBodyStyles));

                this.consoleMessages = this.consoleMessages || document.createElement("ul");
                var cm = this.consoleMessages;
                cm.id = H5.Console.CONSOLE_MESSAGES_ID;

                cm.setAttribute("style", "margin: 0;padding: 0;list-style: none;");

                if (!reinit) {
                    this.highFiveIcon.appendChild(this.highFiveIconPath);
                    this.closeIcon.appendChild(this.closeIconPath);
                    this.closeBtn.appendChild(this.closeIcon);
                    this.closeBtn.appendChild(this.tooltip);

                    this.consoleHeader.appendChild(this.highFiveIcon);
                    this.consoleHeader.appendChild(this.highFiveConsoleLabel);
                    this.consoleHeader.appendChild(this.closeBtn);

                    this.consoleBody.appendChild(cm);

                    this.consoleWrap.appendChild(this.consoleHeader);
                    this.consoleWrap.appendChild(this.consoleBody);

                    document.body.appendChild(this.consoleWrap);

                    this.closeBtn.addEventListener("click", H5.fn.cacheBind(this, this.close));

                    this.closeBtn.addEventListener("mouseover", H5.fn.cacheBind(this, this.showTooltip));
                    this.closeBtn.addEventListener("mouseout", H5.fn.cacheBind(this, this.hideTooltip));
                }
            },
            showTooltip: function () {
                var self = H5.Console.instance;
                self.tooltip.style.right = "20px";
                self.tooltip.style.visibility = "visible";
                self.tooltip.style.opacity = "1";
            },
            hideTooltip: function () {
                var self = H5.Console.instance;
                self.tooltip.style.right = "30px";
                self.tooltip.style.opacity = "0";
            },
            close: function () {
                this.hidden = true;

                this.consoleWrap.style.display = "none";

                if (H5.referenceEquals(H5.Console.position, "horizontal")) {
                    this.unwrapBodyContent();
                } else if (H5.referenceEquals(H5.Console.position, "vertical")) {
                    document.body.removeAttribute("style");
                }
            },
            wrapBodyContent: function () {
                if (document.body == null) {
                    return;
                }

                var bodyStyle = document.defaultView.getComputedStyle(document.body, null);

                var bodyPaddingTop = bodyStyle.paddingTop;
                var bodyPaddingRight = bodyStyle.paddingRight;
                var bodyPaddingBottom = bodyStyle.paddingBottom;
                var bodyPaddingLeft = bodyStyle.paddingLeft;

                var bodyMarginTop = bodyStyle.marginTop;
                var bodyMarginRight = bodyStyle.marginRight;
                var bodyMarginBottom = bodyStyle.marginBottom;
                var bodyMarginLeft = bodyStyle.marginLeft;

                var div = document.createElement("div");
                div.id = H5.Console.BODY_WRAPPER_ID;
                div.setAttribute("style", "height: calc(100vh - " + (this.consoleHeight || "") + " - " + (this.consoleHeaderHeight || "") + ");" + "margin-top: calc(-1 * " + "(" + (((bodyMarginTop || "") + " + " + (bodyPaddingTop || "")) || "") + "));" + "margin-right: calc(-1 * " + "(" + (((bodyMarginRight || "") + " + " + (bodyPaddingRight || "")) || "") + "));" + "margin-left: calc(-1 * " + "(" + (((bodyMarginLeft || "") + " + " + (bodyPaddingLeft || "")) || "") + "));" + "padding-top: calc(" + (((bodyMarginTop || "") + " + " + (bodyPaddingTop || "")) || "") + ");" + "padding-right: calc(" + (((bodyMarginRight || "") + " + " + (bodyPaddingRight || "")) || "") + ");" + "padding-bottom: calc(" + (((bodyMarginBottom || "") + " + " + (bodyPaddingBottom || "")) || "") + ");" + "padding-left: calc(" + (((bodyMarginLeft || "") + " + " + (bodyPaddingLeft || "")) || "") + ");" + "overflow-x: auto;" + "box-sizing: border-box !important;");

                while (document.body.firstChild != null) {
                    div.appendChild(document.body.firstChild);
                }

                document.body.appendChild(div);
            },
            unwrapBodyContent: function () {
                var h5BodyWrap = document.getElementById(H5.Console.BODY_WRAPPER_ID);

                if (h5BodyWrap == null) {
                    return;
                }

                while (h5BodyWrap.firstChild != null) {
                    document.body.insertBefore(h5BodyWrap.firstChild, h5BodyWrap);
                }

                document.body.removeChild(h5BodyWrap);
            },
            buildConsoleMessage: function (message, messageType) {
                var messageItem = document.createElement("li");
                messageItem.setAttribute("style", "padding:5px 10px;border-bottom:1px solid #f0f0f0;position:relative;");

                var messageIcon = document.createElementNS(this.svgNS, "svg");

                var items5 = H5.fn.bind(this, $asm.$.H5.Console.f7)(new (System.Collections.Generic.Dictionary$2(System.String,System.String)).ctor());

                this.setAttributes(messageIcon, items5);

                var color = "#555";

                if (messageType === 2) {
                    color = "#d65050";
                } else if (messageType === 1) {
                    color = "#1800FF";
                }

                var messageIconPath = document.createElementNS(this.svgNS, "path");

                var items6 = new (System.Collections.Generic.Dictionary$2(System.String,System.String)).ctor();

                items6.setItem("d", "M3.8 3.5L.7 6.6s-.1.1-.2.1-.1 0-.2-.1l-.2-.3C0 6.2 0 6.2 0 6.1c0 0 0-.1.1-.1l2.6-2.6L.1.7C0 .7 0 .6 0 .6 0 .5 0 .5.1.4L.4.1c0-.1.1-.1.2-.1s.1 0 .2.1l3.1 3.1s.1.1.1.2-.1.1-.2.1z");
                items6.setItem("fill", color);

                this.setAttributes(messageIconPath, items6);

                messageIcon.appendChild(messageIconPath);

                var messageContainer = document.createElement("div");
                messageContainer.innerText = message;
                messageContainer.setAttribute("style", "color:" + (color || "") + ";white-space:pre;margin-left:12px;line-height:1.4;min-height:18px;");

                messageItem.appendChild(messageIcon);
                messageItem.appendChild(messageContainer);

                return messageItem;
            },
            setAttributes: function (el, attrs) {
                var $t;
                $t = H5.getEnumerator(attrs);
                try {
                    while ($t.moveNext()) {
                        var item = $t.Current;
                        el.setAttribute(item.key, item.value);
                    }
                } finally {
                    if (H5.is($t, System.IDisposable)) {
                        $t.System$IDisposable$Dispose();
                    }
                }
            },
            obj2Css: function (obj) {
                var $t;
                var str = "";

                $t = H5.getEnumerator(obj);
                try {
                    while ($t.moveNext()) {
                        var item = $t.Current;
                        str = (str || "") + (((item.key.toLowerCase() || "") + ":" + (item.value || "") + ";") || "");
                    }
                } finally {
                    if (H5.is($t, System.IDisposable)) {
                        $t.System$IDisposable$Dispose();
                    }
                }

                return str;
            }
        }
    });

    H5.ns("H5.Console", $asm.$);

    H5.apply($asm.$.H5.Console, {
        f1: function (_o1) {
            _o1.add("position", "fixed");
            _o1.add("left", "0");
            _o1.add("bottom", "0");
            _o1.add("padding-top", this.consoleHeaderHeight);
            _o1.add("background-color", "#fff");
            _o1.add("font", "normal normal normal 13px/1 sans-serif");
            _o1.add("color", "#555");
            return _o1;
        },
        f2: function (_o2) {
            _o2.add("position", "absolute");
            _o2.add("top", "0");
            _o2.add("left", "0");
            _o2.add("right", "0");
            _o2.add("height", "35px");
            _o2.add("padding", "9px 15px 7px 10px");
            _o2.add("border-bottom", "1px solid #ccc");
            _o2.add("background-color", "#f3f3f3");
            _o2.add("box-sizing", "border-box");
            return _o2;
        },
        f3: function (_o3) {
            _o3.add("overflow-x", "auto");
            _o3.add("font-family", "Menlo, Monaco, Consolas, 'Courier New', monospace");
            return _o3;
        },
        f4: function (_o4) {
            _o4.add("xmlns", this.svgNS);
            _o4.add("width", "25.5");
            _o4.add("height", "14.4");
            _o4.add("viewBox", "0 0 25.5 14.4");
            _o4.add("style", "margin: 0 3px 3px 0;vertical-align:middle;");
            return _o4;
        },
        f5: function (_o5) {
            _o5.add("xmlns", this.svgNS);
            _o5.add("width", "11.4");
            _o5.add("height", "11.4");
            _o5.add("viewBox", "0 0 11.4 11.4");
            _o5.add("style", "vertical-align: middle;");
            return _o5;
        },
        f6: function (_o6) {
            _o6.add("d", "M11.4 1.4L10 0 5.7 4.3 1.4 0 0 1.4l4.3 4.3L0 10l1.4 1.4 4.3-4.3 4.3 4.3 1.4-1.4-4.3-4.3");
            _o6.add("fill", "#555");
            return _o6;
        },
        f7: function (_o1) {
            _o1.add("xmlns", this.svgNS);
            _o1.add("width", "3.9");
            _o1.add("height", "6.7");
            _o1.add("viewBox", "0 0 3.9 6.7");
            _o1.add("style", "vertical-align:middle;position: absolute;top: 10.5px;");
            return _o1;
        }
    });

    H5.init(function () { H5.Console.initConsoleFunctions(); });
