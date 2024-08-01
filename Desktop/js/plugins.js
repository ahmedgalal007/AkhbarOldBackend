/*
 * jQuery FlexSlider v2.6.3
 * Copyright 2012 WooThemes
 * Contributing Author: Tyler Smith
 */
! function($) {
    var e = !0;
    $.flexslider = function(t, a) {
        var n = $(t);
        n.vars = $.extend({}, $.flexslider.defaults, a);
        var i = n.vars.namespace,
            s = window.navigator && window.navigator.msPointerEnabled && window.MSGesture,
            r = ("ontouchstart" in window || s || window.DocumentTouch && document instanceof DocumentTouch) && n.vars.touch,
            o = "click touchend MSPointerUp keyup",
            l = "",
            c, d = "vertical" === n.vars.direction,
            u = n.vars.reverse,
            v = n.vars.itemWidth > 0,
            p = "fade" === n.vars.animation,
            m = "" !== n.vars.asNavFor,
            f = {};
        $.data(t, "flexslider", n), f = {
            init: function() {
                n.animating = !1, n.currentSlide = parseInt(n.vars.startAt ? n.vars.startAt : 0, 10), isNaN(n.currentSlide) && (n.currentSlide = 0), n.animatingTo = n.currentSlide, n.atEnd = 0 === n.currentSlide || n.currentSlide === n.last, n.containerSelector = n.vars.selector.substr(0, n.vars.selector.search(" ")), n.slides = $(n.vars.selector, n), n.container = $(n.containerSelector, n), n.count = n.slides.length, n.syncExists = $(n.vars.sync).length > 0, "slide" === n.vars.animation && (n.vars.animation = "swing"), n.prop = d ? "top" : "marginLeft", n.args = {}, n.manualPause = !1, n.stopped = !1, n.started = !1, n.startTimeout = null, n.transitions = !n.vars.video && !p && n.vars.useCSS && function() {
                    var e = document.createElement("div"),
                        t = ["perspectiveProperty", "WebkitPerspective", "MozPerspective", "OPerspective", "msPerspective"];
                    for (var a in t)
                        if (void 0 !== e.style[t[a]]) return n.pfx = t[a].replace("Perspective", "").toLowerCase(), n.prop = "-" + n.pfx + "-transform", !0;
                    return !1
                }(), n.ensureAnimationEnd = "", "" !== n.vars.controlsContainer && (n.controlsContainer = $(n.vars.controlsContainer).length > 0 && $(n.vars.controlsContainer)), "" !== n.vars.manualControls && (n.manualControls = $(n.vars.manualControls).length > 0 && $(n.vars.manualControls)), "" !== n.vars.customDirectionNav && (n.customDirectionNav = 2 === $(n.vars.customDirectionNav).length && $(n.vars.customDirectionNav)), n.vars.randomize && (n.slides.sort(function() { return Math.round(Math.random()) - .5 }), n.container.empty().append(n.slides)), n.doMath(), n.setup("init"), n.vars.controlNav && f.controlNav.setup(), n.vars.directionNav && f.directionNav.setup(), n.vars.keyboard && (1 === $(n.containerSelector).length || n.vars.multipleKeyboard) && $(document).bind("keyup", function(e) {
                    var t = e.keyCode;
                    if (!n.animating && (39 === t || 37 === t)) {
                        var a = 39 === t ? n.getTarget("next") : 37 === t ? n.getTarget("prev") : !1;
                        n.flexAnimate(a, n.vars.pauseOnAction)
                    }
                }), n.vars.mousewheel && n.bind("mousewheel", function(e, t, a, i) {
                    e.preventDefault();
                    var s = 0 > t ? n.getTarget("next") : n.getTarget("prev");
                    n.flexAnimate(s, n.vars.pauseOnAction)
                }), n.vars.pausePlay && f.pausePlay.setup(), n.vars.slideshow && n.vars.pauseInvisible && f.pauseInvisible.init(), n.vars.slideshow && (n.vars.pauseOnHover && n.hover(function() { n.manualPlay || n.manualPause || n.pause() }, function() { n.manualPause || n.manualPlay || n.stopped || n.play() }), n.vars.pauseInvisible && f.pauseInvisible.isHidden() || (n.vars.initDelay > 0 ? n.startTimeout = setTimeout(n.play, n.vars.initDelay) : n.play())), m && f.asNav.setup(), r && n.vars.touch && f.touch(), (!p || p && n.vars.smoothHeight) && $(window).bind("resize orientationchange focus", f.resize), n.find("img").attr("draggable", "false"), setTimeout(function() { n.vars.start(n) }, 200)
            },
            asNav: {
                setup: function() {
                    n.asNav = !0, n.animatingTo = Math.floor(n.currentSlide / n.move), n.currentItem = n.currentSlide, n.slides.removeClass(i + "active-slide").eq(n.currentItem).addClass(i + "active-slide"), s ? (t._slider = n, n.slides.each(function() {
                        var e = this;
                        e._gesture = new MSGesture, e._gesture.target = e, e.addEventListener("MSPointerDown", function(e) { e.preventDefault(), e.currentTarget._gesture && e.currentTarget._gesture.addPointer(e.pointerId) }, !1), e.addEventListener("MSGestureTap", function(e) {
                            e.preventDefault();
                            var t = $(this),
                                a = t.index();
                            $(n.vars.asNavFor).data("flexslider").animating || t.hasClass("active") || (n.direction = n.currentItem < a ? "next" : "prev", n.flexAnimate(a, n.vars.pauseOnAction, !1, !0, !0))
                        })
                    })) : n.slides.on(o, function(e) {
                        e.preventDefault();
                        var t = $(this),
                            a = t.index(),
                            s = t.offset().left - $(n).scrollLeft();
                        0 >= s && t.hasClass(i + "active-slide") ? n.flexAnimate(n.getTarget("prev"), !0) : $(n.vars.asNavFor).data("flexslider").animating || t.hasClass(i + "active-slide") || (n.direction = n.currentItem < a ? "next" : "prev", n.flexAnimate(a, n.vars.pauseOnAction, !1, !0, !0))
                    })
                }
            },
            controlNav: {
                setup: function() { n.manualControls ? f.controlNav.setupManual() : f.controlNav.setupPaging() },
                setupPaging: function() {
                    var e = "thumbnails" === n.vars.controlNav ? "control-thumbs" : "control-paging",
                        t = 1,
                        a, s;
                    if (n.controlNavScaffold = $('<ol class="' + i + "control-nav " + i + e + '"></ol>'), n.pagingCount > 1)
                        for (var r = 0; r < n.pagingCount; r++) {
                            s = n.slides.eq(r), void 0 === s.attr("data-thumb-alt") && s.attr("data-thumb-alt", "");
                            var c = "" !== s.attr("data-thumb-alt") ? c = ' alt="' + s.attr("data-thumb-alt") + '"' : "";
                            if (a = "thumbnails" === n.vars.controlNav ? '<img src="' + s.attr("data-thumb") + '"' + c + "/>" : '<a href="#">' + t + "</a>", "thumbnails" === n.vars.controlNav && !0 === n.vars.thumbCaptions) { var d = s.attr("data-thumbcaption"); "" !== d && void 0 !== d && (a += '<span class="' + i + 'caption">' + d + "</span>") }
                            n.controlNavScaffold.append("<li>" + a + "</li>"), t++
                        }
                    n.controlsContainer ? $(n.controlsContainer).append(n.controlNavScaffold) : n.append(n.controlNavScaffold), f.controlNav.set(), f.controlNav.active(), n.controlNavScaffold.delegate("a, img", o, function(e) {
                        if (e.preventDefault(), "" === l || l === e.type) {
                            var t = $(this),
                                a = n.controlNav.index(t);
                            t.hasClass(i + "active") || (n.direction = a > n.currentSlide ? "next" : "prev", n.flexAnimate(a, n.vars.pauseOnAction))
                        }
                        "" === l && (l = e.type), f.setToClearWatchedEvent()
                    })
                },
                setupManual: function() {
                    n.controlNav = n.manualControls, f.controlNav.active(), n.controlNav.bind(o, function(e) {
                        if (e.preventDefault(), "" === l || l === e.type) {
                            var t = $(this),
                                a = n.controlNav.index(t);
                            t.hasClass(i + "active") || (a > n.currentSlide ? n.direction = "next" : n.direction = "prev", n.flexAnimate(a, n.vars.pauseOnAction))
                        }
                        "" === l && (l = e.type), f.setToClearWatchedEvent()
                    })
                },
                set: function() {
                    var e = "thumbnails" === n.vars.controlNav ? "img" : "a";
                    n.controlNav = $("." + i + "control-nav li " + e, n.controlsContainer ? n.controlsContainer : n)
                },
                active: function() { n.controlNav.removeClass(i + "active").eq(n.animatingTo).addClass(i + "active") },
                update: function(e, t) { n.pagingCount > 1 && "add" === e ? n.controlNavScaffold.append($('<li><a href="#">' + n.count + "</a></li>")) : 1 === n.pagingCount ? n.controlNavScaffold.find("li").remove() : n.controlNav.eq(t).closest("li").remove(), f.controlNav.set(), n.pagingCount > 1 && n.pagingCount !== n.controlNav.length ? n.update(t, e) : f.controlNav.active() }
            },
            directionNav: {
                setup: function() {
                    var e = $('<ul class="' + i + 'direction-nav"><li class="' + i + 'nav-prev"><a class="' + i + 'prev" href="#">' + n.vars.prevText + '</a></li><li class="' + i + 'nav-next"><a class="' + i + 'next" href="#">' + n.vars.nextText + "</a></li></ul>");
                    n.customDirectionNav ? n.directionNav = n.customDirectionNav : n.controlsContainer ? ($(n.controlsContainer).append(e), n.directionNav = $("." + i + "direction-nav li a", n.controlsContainer)) : (n.append(e), n.directionNav = $("." + i + "direction-nav li a", n)), f.directionNav.update(), n.directionNav.bind(o, function(e) { e.preventDefault(); var t; "" !== l && l !== e.type || (t = $(this).hasClass(i + "next") ? n.getTarget("next") : n.getTarget("prev"), n.flexAnimate(t, n.vars.pauseOnAction)), "" === l && (l = e.type), f.setToClearWatchedEvent() })
                },
                update: function() {
                    var e = i + "disabled";
                    1 === n.pagingCount ? n.directionNav.addClass(e).attr("tabindex", "-1") : n.vars.animationLoop ? n.directionNav.removeClass(e).removeAttr("tabindex") : 0 === n.animatingTo ? n.directionNav.removeClass(e).filter("." + i + "prev").addClass(e).attr("tabindex", "-1") : n.animatingTo === n.last ? n.directionNav.removeClass(e).filter("." + i + "next").addClass(e).attr("tabindex", "-1") : n.directionNav.removeClass(e).removeAttr("tabindex")
                }
            },
            pausePlay: {
                setup: function() {
                    var e = $('<div class="' + i + 'pauseplay"><a href="#"></a></div>');
                    n.controlsContainer ? (n.controlsContainer.append(e), n.pausePlay = $("." + i + "pauseplay a", n.controlsContainer)) : (n.append(e), n.pausePlay = $("." + i + "pauseplay a", n)), f.pausePlay.update(n.vars.slideshow ? i + "pause" : i + "play"), n.pausePlay.bind(o, function(e) { e.preventDefault(), "" !== l && l !== e.type || ($(this).hasClass(i + "pause") ? (n.manualPause = !0, n.manualPlay = !1, n.pause()) : (n.manualPause = !1, n.manualPlay = !0, n.play())), "" === l && (l = e.type), f.setToClearWatchedEvent() })
                },
                update: function(e) { "play" === e ? n.pausePlay.removeClass(i + "pause").addClass(i + "play").html(n.vars.playText) : n.pausePlay.removeClass(i + "play").addClass(i + "pause").html(n.vars.pauseText) }
            },
            touch: function() {
                function e(e) { e.stopPropagation(), n.animating ? e.preventDefault() : (n.pause(), t._gesture.addPointer(e.pointerId), T = 0, c = d ? n.h : n.w, f = Number(new Date), l = v && u && n.animatingTo === n.last ? 0 : v && u ? n.limit - (n.itemW + n.vars.itemMargin) * n.move * n.animatingTo : v && n.currentSlide === n.last ? n.limit : v ? (n.itemW + n.vars.itemMargin) * n.move * n.currentSlide : u ? (n.last - n.currentSlide + n.cloneOffset) * c : (n.currentSlide + n.cloneOffset) * c) }

                function a(e) {
                    e.stopPropagation();
                    var a = e.target._slider;
                    if (a) {
                        var n = -e.translationX,
                            i = -e.translationY;
                        return T += d ? i : n, m = T, y = d ? Math.abs(T) < Math.abs(-n) : Math.abs(T) < Math.abs(-i), e.detail === e.MSGESTURE_FLAG_INERTIA ? void setImmediate(function() { t._gesture.stop() }) : void((!y || Number(new Date) - f > 500) && (e.preventDefault(), !p && a.transitions && (a.vars.animationLoop || (m = T / (0 === a.currentSlide && 0 > T || a.currentSlide === a.last && T > 0 ? Math.abs(T) / c + 2 : 1)), a.setProps(l + m, "setTouch"))))
                    }
                }

                function i(e) {
                    e.stopPropagation();
                    var t = e.target._slider;
                    if (t) {
                        if (t.animatingTo === t.currentSlide && !y && null !== m) {
                            var a = u ? -m : m,
                                n = a > 0 ? t.getTarget("next") : t.getTarget("prev");
                            t.canAdvance(n) && (Number(new Date) - f < 550 && Math.abs(a) > 50 || Math.abs(a) > c / 2) ? t.flexAnimate(n, t.vars.pauseOnAction) : p || t.flexAnimate(t.currentSlide, t.vars.pauseOnAction, !0)
                        }
                        r = null, o = null, m = null, l = null, T = 0
                    }
                }
                var r, o, l, c, m, f, g, h, S, y = !1,
                    x = 0,
                    b = 0,
                    T = 0;
                s ? (t.style.msTouchAction = "none", t._gesture = new MSGesture, t._gesture.target = t, t.addEventListener("MSPointerDown", e, !1), t._slider = n, t.addEventListener("MSGestureChange", a, !1), t.addEventListener("MSGestureEnd", i, !1)) : (g = function(e) { n.animating ? e.preventDefault() : (window.navigator.msPointerEnabled || 1 === e.touches.length) && (n.pause(), c = d ? n.h : n.w, f = Number(new Date), x = e.touches[0].pageX, b = e.touches[0].pageY, l = v && u && n.animatingTo === n.last ? 0 : v && u ? n.limit - (n.itemW + n.vars.itemMargin) * n.move * n.animatingTo : v && n.currentSlide === n.last ? n.limit : v ? (n.itemW + n.vars.itemMargin) * n.move * n.currentSlide : u ? (n.last - n.currentSlide + n.cloneOffset) * c : (n.currentSlide + n.cloneOffset) * c, r = d ? b : x, o = d ? x : b, t.addEventListener("touchmove", h, !1), t.addEventListener("touchend", S, !1)) }, h = function(e) {
                    x = e.touches[0].pageX, b = e.touches[0].pageY, m = d ? r - b : r - x, y = d ? Math.abs(m) < Math.abs(x - o) : Math.abs(m) < Math.abs(b - o);
                    var t = 500;
                    (!y || Number(new Date) - f > t) && (e.preventDefault(), !p && n.transitions && (n.vars.animationLoop || (m /= 0 === n.currentSlide && 0 > m || n.currentSlide === n.last && m > 0 ? Math.abs(m) / c + 2 : 1), n.setProps(l + m, "setTouch")))
                }, S = function(e) {
                    if (t.removeEventListener("touchmove", h, !1), n.animatingTo === n.currentSlide && !y && null !== m) {
                        var a = u ? -m : m,
                            i = a > 0 ? n.getTarget("next") : n.getTarget("prev");
                        n.canAdvance(i) && (Number(new Date) - f < 550 && Math.abs(a) > 50 || Math.abs(a) > c / 2) ? n.flexAnimate(i, n.vars.pauseOnAction) : p || n.flexAnimate(n.currentSlide, n.vars.pauseOnAction, !0)
                    }
                    t.removeEventListener("touchend", S, !1), r = null, o = null, m = null, l = null
                }, t.addEventListener("touchstart", g, !1))
            },
            resize: function() {!n.animating && n.is(":visible") && (v || n.doMath(), p ? f.smoothHeight() : v ? (n.slides.width(n.computedW), n.update(n.pagingCount), n.setProps()) : d ? (n.viewport.height(n.h), n.setProps(n.h, "setTotal")) : (n.vars.smoothHeight && f.smoothHeight(), n.newSlides.width(n.computedW), n.setProps(n.computedW, "setTotal"))) },
            smoothHeight: function(e) {
                if (!d || p) {
                    var t = p ? n : n.viewport;
                    e ? t.animate({ height: n.slides.eq(n.animatingTo).innerHeight() }, e) : t.innerHeight(n.slides.eq(n.animatingTo).innerHeight())
                }
            },
            sync: function(e) {
                var t = $(n.vars.sync).data("flexslider"),
                    a = n.animatingTo;
                switch (e) {
                    case "animate":
                        t.flexAnimate(a, n.vars.pauseOnAction, !1, !0);
                        break;
                    case "play":
                        t.playing || t.asNav || t.play();
                        break;
                    case "pause":
                        t.pause()
                }
            },
            uniqueID: function(e) {
                return e.filter("[id]").add(e.find("[id]")).each(function() {
                    var e = $(this);
                    e.attr("id", e.attr("id") + "_clone")
                }), e
            },
            pauseInvisible: {
                visProp: null,
                init: function() {
                    var e = f.pauseInvisible.getHiddenProp();
                    if (e) {
                        var t = e.replace(/[H|h]idden/, "") + "visibilitychange";
                        document.addEventListener(t, function() { f.pauseInvisible.isHidden() ? n.startTimeout ? clearTimeout(n.startTimeout) : n.pause() : n.started ? n.play() : n.vars.initDelay > 0 ? setTimeout(n.play, n.vars.initDelay) : n.play() })
                    }
                },
                isHidden: function() { var e = f.pauseInvisible.getHiddenProp(); return e ? document[e] : !1 },
                getHiddenProp: function() {
                    var e = ["webkit", "moz", "ms", "o"];
                    if ("hidden" in document) return "hidden";
                    for (var t = 0; t < e.length; t++)
                        if (e[t] + "Hidden" in document) return e[t] + "Hidden";
                    return null
                }
            },
            setToClearWatchedEvent: function() { clearTimeout(c), c = setTimeout(function() { l = "" }, 3e3) }
        }, n.flexAnimate = function(e, t, a, s, o) {
            if (n.vars.animationLoop || e === n.currentSlide || (n.direction = e > n.currentSlide ? "next" : "prev"), m && 1 === n.pagingCount && (n.direction = n.currentItem < e ? "next" : "prev"), !n.animating && (n.canAdvance(e, o) || a) && n.is(":visible")) {
                if (m && s) {
                    var l = $(n.vars.asNavFor).data("flexslider");
                    if (n.atEnd = 0 === e || e === n.count - 1, l.flexAnimate(e, !0, !1, !0, o), n.direction = n.currentItem < e ? "next" : "prev", l.direction = n.direction, Math.ceil((e + 1) / n.visible) - 1 === n.currentSlide || 0 === e) return n.currentItem = e, n.slides.removeClass(i + "active-slide").eq(e).addClass(i + "active-slide"), !1;
                    n.currentItem = e, n.slides.removeClass(i + "active-slide").eq(e).addClass(i + "active-slide"), e = Math.floor(e / n.visible)
                }
                if (n.animating = !0, n.animatingTo = e, t && n.pause(), n.vars.before(n), n.syncExists && !o && f.sync("animate"), n.vars.controlNav && f.controlNav.active(), v || n.slides.removeClass(i + "active-slide").eq(e).addClass(i + "active-slide"), n.atEnd = 0 === e || e === n.last, n.vars.directionNav && f.directionNav.update(), e === n.last && (n.vars.end(n), n.vars.animationLoop || n.pause()), p) r ? (n.slides.eq(n.currentSlide).css({ opacity: 0, zIndex: 1 }), n.slides.eq(e).css({ opacity: 1, zIndex: 2 }), n.wrapup(c)) : (n.slides.eq(n.currentSlide).css({ zIndex: 1 }).animate({ opacity: 0 }, n.vars.animationSpeed, n.vars.easing), n.slides.eq(e).css({ zIndex: 2 }).animate({ opacity: 1 }, n.vars.animationSpeed, n.vars.easing, n.wrapup));
                else {
                    var c = d ? n.slides.filter(":first").height() : n.computedW,
                        g, h, S;
                    v ? (g = n.vars.itemMargin, S = (n.itemW + g) * n.move * n.animatingTo, h = S > n.limit && 1 !== n.visible ? n.limit : S) : h = 0 === n.currentSlide && e === n.count - 1 && n.vars.animationLoop && "next" !== n.direction ? u ? (n.count + n.cloneOffset) * c : 0 : n.currentSlide === n.last && 0 === e && n.vars.animationLoop && "prev" !== n.direction ? u ? 0 : (n.count + 1) * c : u ? (n.count - 1 - e + n.cloneOffset) * c : (e + n.cloneOffset) * c, n.setProps(h, "", n.vars.animationSpeed), n.transitions ? (n.vars.animationLoop && n.atEnd || (n.animating = !1, n.currentSlide = n.animatingTo), n.container.unbind("webkitTransitionEnd transitionend"), n.container.bind("webkitTransitionEnd transitionend", function() { clearTimeout(n.ensureAnimationEnd), n.wrapup(c) }), clearTimeout(n.ensureAnimationEnd), n.ensureAnimationEnd = setTimeout(function() { n.wrapup(c) }, n.vars.animationSpeed + 100)) : n.container.animate(n.args, n.vars.animationSpeed, n.vars.easing, function() { n.wrapup(c) })
                }
                n.vars.smoothHeight && f.smoothHeight(n.vars.animationSpeed)
            }
        }, n.wrapup = function(e) { p || v || (0 === n.currentSlide && n.animatingTo === n.last && n.vars.animationLoop ? n.setProps(e, "jumpEnd") : n.currentSlide === n.last && 0 === n.animatingTo && n.vars.animationLoop && n.setProps(e, "jumpStart")), n.animating = !1, n.currentSlide = n.animatingTo, n.vars.after(n) }, n.animateSlides = function() {!n.animating && e && n.flexAnimate(n.getTarget("next")) }, n.pause = function() { clearInterval(n.animatedSlides), n.animatedSlides = null, n.playing = !1, n.vars.pausePlay && f.pausePlay.update("play"), n.syncExists && f.sync("pause") }, n.play = function() { n.playing && clearInterval(n.animatedSlides), n.animatedSlides = n.animatedSlides || setInterval(n.animateSlides, n.vars.slideshowSpeed), n.started = n.playing = !0, n.vars.pausePlay && f.pausePlay.update("pause"), n.syncExists && f.sync("play") }, n.stop = function() { n.pause(), n.stopped = !0 }, n.canAdvance = function(e, t) { var a = m ? n.pagingCount - 1 : n.last; return t ? !0 : m && n.currentItem === n.count - 1 && 0 === e && "prev" === n.direction ? !0 : m && 0 === n.currentItem && e === n.pagingCount - 1 && "next" !== n.direction ? !1 : e !== n.currentSlide || m ? n.vars.animationLoop ? !0 : n.atEnd && 0 === n.currentSlide && e === a && "next" !== n.direction ? !1 : !n.atEnd || n.currentSlide !== a || 0 !== e || "next" !== n.direction : !1 }, n.getTarget = function(e) { return n.direction = e, "next" === e ? n.currentSlide === n.last ? 0 : n.currentSlide + 1 : 0 === n.currentSlide ? n.last : n.currentSlide - 1 }, n.setProps = function(e, t, a) {
            var i = function() {
                var a = e ? e : (n.itemW + n.vars.itemMargin) * n.move * n.animatingTo,
                    i = function() {
                        if (v) return "setTouch" === t ? e : u && n.animatingTo === n.last ? 0 : u ? n.limit - (n.itemW + n.vars.itemMargin) * n.move * n.animatingTo : n.animatingTo === n.last ? n.limit : a;
                        switch (t) {
                            case "setTotal":
                                return u ? (n.count - 1 - n.currentSlide + n.cloneOffset) * e : (n.currentSlide + n.cloneOffset) * e;
                            case "setTouch":
                                return u ? e : e;
                            case "jumpEnd":
                                return u ? e : n.count * e;
                            case "jumpStart":
                                return u ? n.count * e : e;
                            default:
                                return e
                        }
                    }();
                return -1 * i + "px"
            }();
            n.transitions && (i = d ? "translate3d(0," + i + ",0)" : "translate3d(" + i + ",0,0)", a = void 0 !== a ? a / 1e3 + "s" : "0s", n.container.css("-" + n.pfx + "-transition-duration", a), n.container.css("transition-duration", a)), n.args[n.prop] = i, (n.transitions || void 0 === a) && n.container.css(n.args), n.container.css("transform", i)
        }, n.setup = function(e) {
            if (p) n.slides.css({ width: "100%", "float": "left", marginRight: "-100%", position: "relative" }), "init" === e && (r ? n.slides.css({ opacity: 0, display: "block", webkitTransition: "opacity " + n.vars.animationSpeed / 1e3 + "s ease", zIndex: 1 }).eq(n.currentSlide).css({ opacity: 1, zIndex: 2 }) : 0 == n.vars.fadeFirstSlide ? n.slides.css({ opacity: 0, display: "block", zIndex: 1 }).eq(n.currentSlide).css({ zIndex: 2 }).css({ opacity: 1 }) : n.slides.css({ opacity: 0, display: "block", zIndex: 1 }).eq(n.currentSlide).css({ zIndex: 2 }).animate({ opacity: 1 }, n.vars.animationSpeed, n.vars.easing)), n.vars.smoothHeight && f.smoothHeight();
            else { var t, a; "init" === e && (n.viewport = $('<div class="' + i + 'viewport"></div>').css({ overflow: "hidden", position: "relative" }).appendTo(n).append(n.container), n.cloneCount = 0, n.cloneOffset = 0, u && (a = $.makeArray(n.slides).reverse(), n.slides = $(a), n.container.empty().append(n.slides))), n.vars.animationLoop && !v && (n.cloneCount = 2, n.cloneOffset = 1, "init" !== e && n.container.find(".clone").remove(), n.container.append(f.uniqueID(n.slides.first().clone().addClass("clone")).attr("aria-hidden", "true")).prepend(f.uniqueID(n.slides.last().clone().addClass("clone")).attr("aria-hidden", "true"))), n.newSlides = $(n.vars.selector, n), t = u ? n.count - 1 - n.currentSlide + n.cloneOffset : n.currentSlide + n.cloneOffset, d && !v ? (n.container.height(200 * (n.count + n.cloneCount) + "%").css("position", "absolute").width("100%"), setTimeout(function() { n.newSlides.css({ display: "block" }), n.doMath(), n.viewport.height(n.h), n.setProps(t * n.h, "init") }, "init" === e ? 100 : 0)) : (n.container.width(200 * (n.count + n.cloneCount) + "%"), n.setProps(t * n.computedW, "init"), setTimeout(function() { n.doMath(), n.newSlides.css({ width: n.computedW, marginRight: n.computedM, "float": "left", display: "block" }), n.vars.smoothHeight && f.smoothHeight() }, "init" === e ? 100 : 0)) }
            v || n.slides.removeClass(i + "active-slide").eq(n.currentSlide).addClass(i + "active-slide"), n.vars.init(n)
        }, n.doMath = function() {
            var e = n.slides.first(),
                t = n.vars.itemMargin,
                a = n.vars.minItems,
                i = n.vars.maxItems;
            n.w = void 0 === n.viewport ? n.width() : n.viewport.width(), n.h = e.height(), n.boxPadding = e.outerWidth() - e.width(), v ? (n.itemT = n.vars.itemWidth + t, n.itemM = t, n.minW = a ? a * n.itemT : n.w, n.maxW = i ? i * n.itemT - t : n.w, n.itemW = n.minW > n.w ? (n.w - t * (a - 1)) / a : n.maxW < n.w ? (n.w - t * (i - 1)) / i : n.vars.itemWidth > n.w ? n.w : n.vars.itemWidth, n.visible = Math.floor(n.w / n.itemW), n.move = n.vars.move > 0 && n.vars.move < n.visible ? n.vars.move : n.visible, n.pagingCount = Math.ceil((n.count - n.visible) / n.move + 1), n.last = n.pagingCount - 1, n.limit = 1 === n.pagingCount ? 0 : n.vars.itemWidth > n.w ? n.itemW * (n.count - 1) + t * (n.count - 1) : (n.itemW + t) * n.count - n.w - t) : (n.itemW = n.w, n.itemM = t, n.pagingCount = n.count, n.last = n.count - 1), n.computedW = n.itemW - n.boxPadding, n.computedM = n.itemM
        }, n.update = function(e, t) { n.doMath(), v || (e < n.currentSlide ? n.currentSlide += 1 : e <= n.currentSlide && 0 !== e && (n.currentSlide -= 1), n.animatingTo = n.currentSlide), n.vars.controlNav && !n.manualControls && ("add" === t && !v || n.pagingCount > n.controlNav.length ? f.controlNav.update("add") : ("remove" === t && !v || n.pagingCount < n.controlNav.length) && (v && n.currentSlide > n.last && (n.currentSlide -= 1, n.animatingTo -= 1), f.controlNav.update("remove", n.last))), n.vars.directionNav && f.directionNav.update() }, n.addSlide = function(e, t) {
            var a = $(e);
            n.count += 1, n.last = n.count - 1, d && u ? void 0 !== t ? n.slides.eq(n.count - t).after(a) : n.container.prepend(a) : void 0 !== t ? n.slides.eq(t).before(a) : n.container.append(a), n.update(t, "add"), n.slides = $(n.vars.selector + ":not(.clone)", n), n.setup(), n.vars.added(n)
        }, n.removeSlide = function(e) {
            var t = isNaN(e) ? n.slides.index($(e)) : e;
            n.count -= 1, n.last = n.count - 1, isNaN(e) ? $(e, n.slides).remove() : d && u ? n.slides.eq(n.last).remove() : n.slides.eq(e).remove(), n.doMath(), n.update(t, "remove"), n.slides = $(n.vars.selector + ":not(.clone)", n), n.setup(), n.vars.removed(n)
        }, f.init()
    }, $(window).blur(function(t) { e = !1 }).focus(function(t) { e = !0 }), $.flexslider.defaults = { namespace: "flex-", selector: ".slides > li", animation: "fade", easing: "swing", direction: "horizontal", reverse: !1, animationLoop: !0, smoothHeight: !1, startAt: 0, slideshow: !0, slideshowSpeed: 7e3, animationSpeed: 600, initDelay: 0, randomize: !1, fadeFirstSlide: !0, thumbCaptions: !1, pauseOnAction: !0, pauseOnHover: !1, pauseInvisible: !0, useCSS: !0, touch: !0, video: !1, controlNav: !0, directionNav: !0, prevText: "Previous", nextText: "Next", keyboard: !0, multipleKeyboard: !1, mousewheel: !1, pausePlay: !1, pauseText: "Pause", playText: "Play", controlsContainer: "", manualControls: "", customDirectionNav: "", sync: "", asNavFor: "", itemWidth: 0, itemMargin: 0, minItems: 1, maxItems: 0, move: 0, allowOneSlide: !0, start: function() {}, before: function() {}, after: function() {}, end: function() {}, added: function() {}, removed: function() {}, init: function() {} }, $.fn.flexslider = function(e) {
        if (void 0 === e && (e = {}), "object" == typeof e) return this.each(function() {
            var t = $(this),
                a = e.selector ? e.selector : ".slides > li",
                n = t.find(a);
            1 === n.length && e.allowOneSlide === !1 || 0 === n.length ? (n.fadeIn(400), e.start && e.start(t)) : void 0 === t.data("flexslider") && new $.flexslider(this, e)
        });
        var t = $(this).data("flexslider");
        switch (e) {
            case "play":
                t.play();
                break;
            case "pause":
                t.pause();
                break;
            case "stop":
                t.stop();
                break;
            case "next":
                t.flexAnimate(t.getTarget("next"), !0);
                break;
            case "prev":
            case "previous":
                t.flexAnimate(t.getTarget("prev"), !0);
                break;
            default:
                "number" == typeof e && t.flexAnimate(e, !0)
        }
    }
}(jQuery);


$('.flexslider').flexslider({
    animation: "fade",
    controlNav: true
});


/*
 * jQuery Easing v1.3 - https://gsgd.co.uk/sandbox/jquery/easing/
 *
 * Uses the built in easing capabilities added In jQuery 1.1
 * to offer multiple easing options
 *
 * TERMS OF USE - jQuery Easing
 * 
 * Open source under the BSD License. 
 * 
 * Copyright © 2008 George McGinley Smith
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list of 
 * conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list 
 * of conditions and the following disclaimer in the documentation and/or other materials 
 * provided with the distribution.
 * 
 * Neither the name of the author nor the names of contributors may be used to endorse 
 * or promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
 *  COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 *  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
 *  GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED 
 * AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 *  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED 
 * OF THE POSSIBILITY OF SUCH DAMAGE. 
 *
 */

(function(factory) { if (typeof define === "function" && define.amd) { define(["jquery"], function($) { return factory($) }) } else if (typeof module === "object" && typeof module.exports === "object") { exports = factory(require("jquery")) } else { factory(jQuery) } })(function($) {
    $.easing["jswing"] = $.easing["swing"];
    var pow = Math.pow,
        sqrt = Math.sqrt,
        sin = Math.sin,
        cos = Math.cos,
        PI = Math.PI,
        c1 = 1.70158,
        c2 = c1 * 1.525,
        c3 = c1 + 1,
        c4 = 2 * PI / 3,
        c5 = 2 * PI / 4.5;

    function bounceOut(x) {
        var n1 = 7.5625,
            d1 = 2.75;
        if (x < 1 / d1) { return n1 * x * x } else if (x < 2 / d1) { return n1 * (x -= 1.5 / d1) * x + .75 } else if (x < 2.5 / d1) { return n1 * (x -= 2.25 / d1) * x + .9375 } else { return n1 * (x -= 2.625 / d1) * x + .984375 }
    }
    $.extend($.easing, { def: "easeOutQuad", swing: function(x) { return $.easing[$.easing.def](x) }, easeInQuad: function(x) { return x * x }, easeOutQuad: function(x) { return 1 - (1 - x) * (1 - x) }, easeInOutQuad: function(x) { return x < .5 ? 2 * x * x : 1 - pow(-2 * x + 2, 2) / 2 }, easeInCubic: function(x) { return x * x * x }, easeOutCubic: function(x) { return 1 - pow(1 - x, 3) }, easeInOutCubic: function(x) { return x < .5 ? 4 * x * x * x : 1 - pow(-2 * x + 2, 3) / 2 }, easeInQuart: function(x) { return x * x * x * x }, easeOutQuart: function(x) { return 1 - pow(1 - x, 4) }, easeInOutQuart: function(x) { return x < .5 ? 8 * x * x * x * x : 1 - pow(-2 * x + 2, 4) / 2 }, easeInQuint: function(x) { return x * x * x * x * x }, easeOutQuint: function(x) { return 1 - pow(1 - x, 5) }, easeInOutQuint: function(x) { return x < .5 ? 16 * x * x * x * x * x : 1 - pow(-2 * x + 2, 5) / 2 }, easeInSine: function(x) { return 1 - cos(x * PI / 2) }, easeOutSine: function(x) { return sin(x * PI / 2) }, easeInOutSine: function(x) { return -(cos(PI * x) - 1) / 2 }, easeInExpo: function(x) { return x === 0 ? 0 : pow(2, 10 * x - 10) }, easeOutExpo: function(x) { return x === 1 ? 1 : 1 - pow(2, -10 * x) }, easeInOutExpo: function(x) { return x === 0 ? 0 : x === 1 ? 1 : x < .5 ? pow(2, 20 * x - 10) / 2 : (2 - pow(2, -20 * x + 10)) / 2 }, easeInCirc: function(x) { return 1 - sqrt(1 - pow(x, 2)) }, easeOutCirc: function(x) { return sqrt(1 - pow(x - 1, 2)) }, easeInOutCirc: function(x) { return x < .5 ? (1 - sqrt(1 - pow(2 * x, 2))) / 2 : (sqrt(1 - pow(-2 * x + 2, 2)) + 1) / 2 }, easeInElastic: function(x) { return x === 0 ? 0 : x === 1 ? 1 : -pow(2, 10 * x - 10) * sin((x * 10 - 10.75) * c4) }, easeOutElastic: function(x) { return x === 0 ? 0 : x === 1 ? 1 : pow(2, -10 * x) * sin((x * 10 - .75) * c4) + 1 }, easeInOutElastic: function(x) { return x === 0 ? 0 : x === 1 ? 1 : x < .5 ? -(pow(2, 20 * x - 10) * sin((20 * x - 11.125) * c5)) / 2 : pow(2, -20 * x + 10) * sin((20 * x - 11.125) * c5) / 2 + 1 }, easeInBack: function(x) { return c3 * x * x * x - c1 * x * x }, easeOutBack: function(x) { return 1 + c3 * pow(x - 1, 3) + c1 * pow(x - 1, 2) }, easeInOutBack: function(x) { return x < .5 ? pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2) / 2 : (pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2 }, easeInBounce: function(x) { return 1 - bounceOut(1 - x) }, easeOutBounce: bounceOut, easeInOutBounce: function(x) { return x < .5 ? (1 - bounceOut(1 - 2 * x)) / 2 : (1 + bounceOut(2 * x - 1)) / 2 } })
});

/*
 *
 * TERMS OF USE - EASING EQUATIONS
 * 
 * Open source under the BSD License. 
 * 
 * Copyright © 2001 Robert Penner
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list of 
 * conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list 
 * of conditions and the following disclaimer in the documentation and/or other materials 
 * provided with the distribution.
 * 
 * Neither the name of the author nor the names of contributors may be used to endorse 
 * or promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
 *  COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 *  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
 *  GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED 
 * AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 *  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED 
 * OF THE POSSIBILITY OF SUCH DAMAGE. 
 *
 */

/*
Plugin: jQuery Parallax
Version 1.1.3
Author: Ian Lunn
Twitter: @IanLunn
Author URL: https://www.ianlunn.co.uk/
Plugin URL: https://www.ianlunn.co.uk/plugins/jquery-parallax/

Dual licensed under the MIT and GPL licenses:
https://www.opensource.org/licenses/mit-license.php
https://www.gnu.org/licenses/gpl.html
*/

(function($) {
    var $window = $(window);
    var windowHeight = $window.height();
    $window.resize(function() { windowHeight = $window.height() });
    $.fn.parallax = function(xpos, speedFactor, outerHeight) {
        var $this = $(this);
        var getHeight;
        var firstTop;
        var paddingTop = 0;
        $this.each(function() { firstTop = $this.offset().top });
        if (outerHeight) { getHeight = function(jqo) { return jqo.outerHeight(true) } } else { getHeight = function(jqo) { return jqo.height() } }
        if (arguments.length < 1 || xpos === null) xpos = "50%";
        if (arguments.length < 2 || speedFactor === null) speedFactor = 0.1;
        if (arguments.length < 3 || outerHeight === null) outerHeight = true;

        function update() {
            var pos = $window.scrollTop();
            $this.each(function() {
                var $element = $(this);
                var top = $element.offset().top;
                var height = getHeight($element);
                if (top + height < pos || top > pos + windowHeight) { return }
                $this.css('backgroundPosition', xpos + " " + Math.round((firstTop - pos) * speedFactor) + "px")
            })
        }
        $window.bind('scroll', update).resize(update);
        update()
    }
})(jQuery);


//
// SmoothScroll for websites v1.4.4 (Balazs Galambosi)
// https://www.smoothscroll.net/
//
// Licensed under the terms of the MIT license.
//
// You may use it in your theme if you credit me. 
// It is also free to use on any individual website.
//
// Exception:
// The only restriction is to not publish any  
// extension for browsers or native application
// without getting a written permission first.
//

! function() {
    function e() { z.keyboardSupport && m("keydown", a) }

    function t() {
        if (!A && document.body) {
            A = !0;
            var t = document.body,
                o = document.documentElement,
                n = window.innerHeight,
                r = t.scrollHeight;
            if (B = document.compatMode.indexOf("CSS") >= 0 ? o : t, D = t, e(), top != self) X = !0;
            else if (r > n && (t.offsetHeight <= n || o.offsetHeight <= n)) {
                var a = document.createElement("div");
                a.style.cssText = "position:absolute; z-index:-10000; top:0; left:0; right:0; height:" + B.scrollHeight + "px", document.body.appendChild(a);
                var i;
                T = function() { i || (i = setTimeout(function() { L || (a.style.height = "0", a.style.height = B.scrollHeight + "px", i = null) }, 500)) }, setTimeout(T, 10), m("resize", T);
                var l = { attributes: !0, childList: !0, characterData: !1 };
                if (M = new W(T), M.observe(t, l), B.offsetHeight <= n) {
                    var c = document.createElement("div");
                    c.style.clear = "both", t.appendChild(c)
                }
            }
            z.fixedBackground || L || (t.style.backgroundAttachment = "scroll", o.style.backgroundAttachment = "scroll")
        }
    }

    function o() { M && M.disconnect(), w(_, r), w("mousedown", i), w("keydown", a), w("resize", T), w("load", t) }

    function n(e, t, o) {
        if (p(t, o), 1 != z.accelerationMax) {
            var n = Date.now(),
                r = n - j;
            if (r < z.accelerationDelta) {
                var a = (1 + 50 / r) / 2;
                a > 1 && (a = Math.min(a, z.accelerationMax), t *= a, o *= a)
            }
            j = Date.now()
        }
        if (q.push({ x: t, y: o, lastX: 0 > t ? .99 : -.99, lastY: 0 > o ? .99 : -.99, start: Date.now() }), !P) {
            var i = e === document.body,
                l = function(n) {
                    for (var r = Date.now(), a = 0, c = 0, u = 0; u < q.length; u++) {
                        var d = q[u],
                            s = r - d.start,
                            f = s >= z.animationTime,
                            m = f ? 1 : s / z.animationTime;
                        z.pulseAlgorithm && (m = x(m));
                        var w = d.x * m - d.lastX >> 0,
                            h = d.y * m - d.lastY >> 0;
                        a += w, c += h, d.lastX += w, d.lastY += h, f && (q.splice(u, 1), u--)
                    }
                    i ? window.scrollBy(a, c) : (a && (e.scrollLeft += a), c && (e.scrollTop += c)), t || o || (q = []), q.length ? V(l, e, 1e3 / z.frameRate + 1) : P = !1
                };
            V(l, e, 0), P = !0
        }
    }

    function r(e) {
        A || t();
        var o = e.target,
            r = u(o);
        if (!r || e.defaultPrevented || e.ctrlKey) return !0;
        if (h(D, "embed") || h(o, "embed") && /\.pdf/i.test(o.src) || h(D, "object") || o.shadowRoot) return !0;
        var a = -e.wheelDeltaX || e.deltaX || 0,
            i = -e.wheelDeltaY || e.deltaY || 0;
        return O && (e.wheelDeltaX && b(e.wheelDeltaX, 120) && (a = -120 * (e.wheelDeltaX / Math.abs(e.wheelDeltaX))), e.wheelDeltaY && b(e.wheelDeltaY, 120) && (i = -120 * (e.wheelDeltaY / Math.abs(e.wheelDeltaY)))), a || i || (i = -e.wheelDelta || 0), 1 === e.deltaMode && (a *= 40, i *= 40), !(z.touchpadSupport || !v(i)) || (Math.abs(a) > 1.2 && (a *= z.stepSize / 120), Math.abs(i) > 1.2 && (i *= z.stepSize / 120), n(r, a, i), e.preventDefault(), void l())
    }

    function a(e) {
        var t = e.target,
            o = e.ctrlKey || e.altKey || e.metaKey || e.shiftKey && e.keyCode !== K.spacebar;
        document.body.contains(D) || (D = document.activeElement);
        var r = /^(textarea|select|embed|object)$/i,
            a = /^(button|submit|radio|checkbox|file|color|image)$/i;
        if (e.defaultPrevented || r.test(t.nodeName) || h(t, "input") && !a.test(t.type) || h(D, "video") || g(e) || t.isContentEditable || o) return !0;
        if ((h(t, "button") || h(t, "input") && a.test(t.type)) && e.keyCode === K.spacebar) return !0;
        if (h(t, "input") && "radio" == t.type && R[e.keyCode]) return !0;
        var i, c = 0,
            d = 0,
            s = u(D),
            f = s.clientHeight;
        switch (s == document.body && (f = window.innerHeight), e.keyCode) {
            case K.up:
                d = -z.arrowScroll;
                break;
            case K.down:
                d = z.arrowScroll;
                break;
            case K.spacebar:
                i = e.shiftKey ? 1 : -1, d = -i * f * .9;
                break;
            case K.pageup:
                d = .9 * -f;
                break;
            case K.pagedown:
                d = .9 * f;
                break;
            case K.home:
                d = -s.scrollTop;
                break;
            case K.end:
                var m = s.scrollHeight - s.scrollTop - f;
                d = m > 0 ? m + 10 : 0;
                break;
            case K.left:
                c = -z.arrowScroll;
                break;
            case K.right:
                c = z.arrowScroll;
                break;
            default:
                return !0
        }
        n(s, c, d), e.preventDefault(), l()
    }

    function i(e) { D = e.target }

    function l() { clearTimeout(E), E = setInterval(function() { I = {} }, 1e3) }

    function c(e, t) { for (var o = e.length; o--;) I[F(e[o])] = t; return t }

    function u(e) {
        var t = [],
            o = document.body,
            n = B.scrollHeight;
        do {
            var r = I[F(e)];
            if (r) return c(t, r);
            if (t.push(e), n === e.scrollHeight) {
                var a = s(B) && s(o),
                    i = a || f(B);
                if (X && d(B) || !X && i) return c(t, $())
            } else if (d(e) && f(e)) return c(t, e)
        } while (e = e.parentElement)
    }

    function d(e) { return e.clientHeight + 10 < e.scrollHeight }

    function s(e) { var t = getComputedStyle(e, "").getPropertyValue("overflow-y"); return "hidden" !== t }

    function f(e) { var t = getComputedStyle(e, "").getPropertyValue("overflow-y"); return "scroll" === t || "auto" === t }

    function m(e, t) { window.addEventListener(e, t, !1) }

    function w(e, t) { window.removeEventListener(e, t, !1) }

    function h(e, t) { return (e.nodeName || "").toLowerCase() === t.toLowerCase() }

    function p(e, t) { e = e > 0 ? 1 : -1, t = t > 0 ? 1 : -1, Y.x === e && Y.y === t || (Y.x = e, Y.y = t, q = [], j = 0) }

    function v(e) { return e ? (N.length || (N = [e, e, e]), e = Math.abs(e), N.push(e), N.shift(), clearTimeout(C), C = setTimeout(function() { window.localStorage && (localStorage.SS_deltaBuffer = N.join(",")) }, 1e3), !y(120) && !y(100)) : void 0 }

    function b(e, t) { return Math.floor(e / t) == e / t }

    function y(e) { return b(N[0], e) && b(N[1], e) && b(N[2], e) }

    function g(e) {
        var t = e.target,
            o = !1;
        if (-1 != document.URL.indexOf("www.youtube.com/watch"))
            do
                if (o = t.classList && t.classList.contains("html5-video-controls")) break;
        while (t = t.parentNode);
        return o
    }

    function S(e) { var t, o, n; return e *= z.pulseScale, 1 > e ? t = e - (1 - Math.exp(-e)) : (o = Math.exp(-1), e -= 1, n = 1 - Math.exp(-e), t = o + n * (1 - o)), t * z.pulseNormalize }

    function x(e) { return e >= 1 ? 1 : 0 >= e ? 0 : (1 == z.pulseNormalize && (z.pulseNormalize /= S(1)), S(e)) }

    function k(e) { for (var t in e) H.hasOwnProperty(t) && (z[t] = e[t]) }
    var D, M, T, E, C, H = { frameRate: 150, animationTime: 400, stepSize: 100, pulseAlgorithm: !0, pulseScale: 4, pulseNormalize: 1, accelerationDelta: 50, accelerationMax: 3, keyboardSupport: !0, arrowScroll: 50, touchpadSupport: !1, fixedBackground: !0, excluded: "" },
        z = H,
        L = !1,
        X = !1,
        Y = { x: 0, y: 0 },
        A = !1,
        B = document.documentElement,
        N = [],
        O = /^Mac/.test(navigator.platform),
        K = { left: 37, up: 38, right: 39, down: 40, spacebar: 32, pageup: 33, pagedown: 34, end: 35, home: 36 },
        R = { 37: 1, 38: 1, 39: 1, 40: 1 },
        q = [],
        P = !1,
        j = Date.now(),
        F = function() { var e = 0; return function(t) { return t.uniqueID || (t.uniqueID = e++) } }(),
        I = {};
    window.localStorage && localStorage.SS_deltaBuffer && (N = localStorage.SS_deltaBuffer.split(","));
    var _, V = function() { return window.requestAnimationFrame || window.webkitRequestAnimationFrame || window.mozRequestAnimationFrame || function(e, t, o) { window.setTimeout(e, o || 1e3 / 60) } }(),
        W = window.MutationObserver || window.WebKitMutationObserver || window.MozMutationObserver,
        $ = function() {
            var e;
            return function() {
                if (!e) {
                    var t = document.createElement("div");
                    t.style.cssText = "height:10000px;width:1px;", document.body.appendChild(t);
                    var o = document.body.scrollTop;
                    document.documentElement.scrollTop, window.scrollBy(0, 3), e = document.body.scrollTop != o ? document.body : document.documentElement, window.scrollBy(0, -3), document.body.removeChild(t)
                }
                return e
            }
        }(),
        U = window.navigator.userAgent,
        G = /Edge/.test(U),
        J = /chrome/i.test(U) && !G,
        Q = /safari/i.test(U) && !G,
        Z = /mobile/i.test(U),
        ee = /Windows NT 6.1/i.test(U) && /rv:11/i.test(U),
        te = (J || Q || ee) && !Z;
    "onwheel" in document.createElement("div") ? _ = "wheel" : "onmousewheel" in document.createElement("div") && (_ = "mousewheel"), _ && te && (m(_, r), m("mousedown", i), m("load", t)), k.destroy = o, window.SmoothScrollOptions && k(window.SmoothScrollOptions), "function" == typeof define && define.amd ? define(function() { return k }) : "object" == typeof exports ? module.exports = k : window.SmoothScroll = k
}();

/* 
 * jQuery - Easy Ticker plugin - v2.0
 * https://www.aakashweb.com/
 * Copyright 2014, Aakash Chakravarthy
 * Released under the MIT License.
 */

! function(t, e, o, n) {
    function i(n, i) {
        function s() { m.elem.children().css("margin", 0).children().css("margin", 0), m.elem.css({ position: "relative", height: m.opts.height, overflow: "hidden" }), m.targ.css({ position: "absolute", margin: 0 }), setInterval(function() { f() }, 100) }

        function c() { m.timer = setInterval(function() { 1 == m.winFocus && a(m.opts.direction) }, m.opts.interval), t(m.opts.controls.toggle).addClass("breaking-ribbon").html(m.opts.controls.stopText) }

        function l() { clearInterval(m.timer), m.timer = 0, t(m.opts.controls.toggle).removeClass("breaking-ribbon").html(m.opts.controls.playText) }

        function a(t) {
            var e, o, n;
            if (m.elem.is(":visible")) {
                "up" == t ? (e = ":first-child", o = "-=", n = "appendTo") : (e = ":last-child", o = "+=", n = "prependTo");
                var i = m.targ.children(e),
                    s = i.outerHeight();
                m.targ.stop(!0, !0).animate({ top: o + s + "px" }, m.opts.speed, m.opts.easing, function() { i.hide()[n](m.targ).fadeIn(), m.targ.css("top", 0), f() })
            }
        }

        function u(t) { l(), a("up" == t ? "up" : "down") }

        function p() {
            var e = 0,
                o = m.elem.css("display");
            m.elem.css("display", "block"), m.targ.children().each(function() { e += t(this).outerHeight() }), m.elem.css({ display: o, height: e })
        }

        function h(e) {
            var o = 0;
            m.targ.children(":lt(" + m.opts.visible + ")").each(function() { o += t(this).outerHeight() }), 1 == e ? m.elem.stop(!0, !0).animate({ height: o }, m.opts.speed) : m.elem.css("height", o)
        }

        function f() { "auto" == m.opts.height && 0 != m.opts.visible ? (anim = "init" == arguments.callee.caller.name ? 0 : 1, h(anim)) : "auto" == m.opts.height && p() }
        var m = this;
        return m.opts = t.extend({}, r, i), m.elem = t(n), m.targ = t(n).children(":first-child"), m.timer = 0, m.mHover = 0, m.winFocus = 1, s(), c(), t([e, o]).off("focus.jqet").on("focus.jqet", function() { m.winFocus = 1 }).off("blur.jqet").on("blur.jqet", function() { m.winFocus = 0 }), 1 == m.opts.mousePause && m.elem.mouseenter(function() { m.timerTemp = m.timer, l() }).mouseleave(function() { 0 !== m.timerTemp && c() }), t(m.opts.controls.up).on("click", function(t) { t.preventDefault(), u("up") }), t(m.opts.controls.down).on("click", function(t) { t.preventDefault(), u("down") }), t(m.opts.controls.toggle).on("click", function(t) { t.preventDefault(), 0 == m.timer ? c() : l() }), { up: function() { u("up") }, down: function() { u("down") }, start: c, stop: l, options: m.opts }
    }
    var s = "easyTicker",
        r = { direction: "up", easing: "swing", speed: "slow", interval: 2e3, height: "auto", visible: 0, mousePause: 1, controls: { up: "", down: "", toggle: "", playText: "Play", stopText: "Stop" } };
    t.fn[s] = function(e) { return this.each(function() { t.data(this, s) || t.data(this, s, new i(this, e)) }) }
}(jQuery, window, document);

/* == malihu jquery custom scrollbar plugin == Version: 3.1.5, License: MIT License (MIT) */
! function(e) { "function" == typeof define && define.amd ? define(["jquery"], e) : "undefined" != typeof module && module.exports ? module.exports = e : e(jQuery, window, document) }(function(e) {
    ! function(t) {
        var o = "function" == typeof define && define.amd,
            a = "undefined" != typeof module && module.exports,
            n = "https:" == document.location.protocol ? "https:" : "https:",
            i = "cdnjs.cloudflare.com/ajax/libs/jquery-mousewheel/3.1.13/jquery.mousewheel.min.js";
        o || (a ? require("jquery-mousewheel")(e) : e.event.special.mousewheel || e("head").append(decodeURI("%3Cscript src=" + n + "//" + i + "%3E%3C/script%3E"))), t()
    }(function() {
        var t, o = "mCustomScrollbar",
            a = "mCS",
            n = ".mCustomScrollbar",
            i = { setTop: 0, setLeft: 0, axis: "y", scrollbarPosition: "inside", scrollInertia: 950, autoDraggerLength: !0, alwaysShowScrollbar: 0, snapOffset: 0, mouseWheel: { enable: !0, scrollAmount: "auto", axis: "y", deltaFactor: "auto", disableOver: ["select", "option", "keygen", "datalist", "textarea"] }, scrollButtons: { scrollType: "stepless", scrollAmount: "auto" }, keyboard: { enable: !0, scrollType: "stepless", scrollAmount: "auto" }, contentTouchScroll: 25, documentTouchScroll: !0, advanced: { autoScrollOnFocus: "input,textarea,select,button,datalist,keygen,a[tabindex],area,object,[contenteditable='true']", updateOnContentResize: !0, updateOnImageLoad: "auto", autoUpdateTimeout: 60 }, theme: "light", callbacks: { onTotalScrollOffset: 0, onTotalScrollBackOffset: 0, alwaysTriggerOffsets: !0 } },
            r = 0,
            l = {},
            s = window.attachEvent && !window.addEventListener ? 1 : 0,
            c = !1,
            d = ["mCSB_dragger_onDrag", "mCSB_scrollTools_onDrag", "mCS_img_loaded", "mCS_disabled", "mCS_destroyed", "mCS_no_scrollbar", "mCS-autoHide", "mCS-dir-rtl", "mCS_no_scrollbar_y", "mCS_no_scrollbar_x", "mCS_y_hidden", "mCS_x_hidden", "mCSB_draggerContainer", "mCSB_buttonUp", "mCSB_buttonDown", "mCSB_buttonLeft", "mCSB_buttonRight"],
            u = {
                init: function(t) {
                    var t = e.extend(!0, {}, i, t),
                        o = f.call(this);
                    if (t.live) {
                        var s = t.liveSelector || this.selector || n,
                            c = e(s);
                        if ("off" === t.live) return void m(s);
                        l[s] = setTimeout(function() { c.mCustomScrollbar(t), "once" === t.live && c.length && m(s) }, 500)
                    } else m(s);
                    return t.setWidth = t.set_width ? t.set_width : t.setWidth, t.setHeight = t.set_height ? t.set_height : t.setHeight, t.axis = t.horizontalScroll ? "x" : p(t.axis), t.scrollInertia = t.scrollInertia > 0 && t.scrollInertia < 17 ? 17 : t.scrollInertia, "object" != typeof t.mouseWheel && 1 == t.mouseWheel && (t.mouseWheel = { enable: !0, scrollAmount: "auto", axis: "y", preventDefault: !1, deltaFactor: "auto", normalizeDelta: !1, invert: !1 }), t.mouseWheel.scrollAmount = t.mouseWheelPixels ? t.mouseWheelPixels : t.mouseWheel.scrollAmount, t.mouseWheel.normalizeDelta = t.advanced.normalizeMouseWheelDelta ? t.advanced.normalizeMouseWheelDelta : t.mouseWheel.normalizeDelta, t.scrollButtons.scrollType = g(t.scrollButtons.scrollType), h(t), e(o).each(function() {
                        var o = e(this);
                        if (!o.data(a)) {
                            o.data(a, { idx: ++r, opt: t, scrollRatio: { y: null, x: null }, overflowed: null, contentReset: { y: null, x: null }, bindEvents: !1, tweenRunning: !1, sequential: {}, langDir: o.css("direction"), cbOffsets: null, trigger: null, poll: { size: { o: 0, n: 0 }, img: { o: 0, n: 0 }, change: { o: 0, n: 0 } } });
                            var n = o.data(a),
                                i = n.opt,
                                l = o.data("mcs-axis"),
                                s = o.data("mcs-scrollbar-position"),
                                c = o.data("mcs-theme");
                            l && (i.axis = l), s && (i.scrollbarPosition = s), c && (i.theme = c, h(i)), v.call(this), n && i.callbacks.onCreate && "function" == typeof i.callbacks.onCreate && i.callbacks.onCreate.call(this), e("#mCSB_" + n.idx + "_container img:not(." + d[2] + ")").addClass(d[2]), u.update.call(null, o)
                        }
                    })
                },
                update: function(t, o) {
                    var n = t || f.call(this);
                    return e(n).each(function() {
                        var t = e(this);
                        if (t.data(a)) {
                            var n = t.data(a),
                                i = n.opt,
                                r = e("#mCSB_" + n.idx + "_container"),
                                l = e("#mCSB_" + n.idx),
                                s = [e("#mCSB_" + n.idx + "_dragger_vertical"), e("#mCSB_" + n.idx + "_dragger_horizontal")];
                            if (!r.length) return;
                            n.tweenRunning && Q(t), o && n && i.callbacks.onBeforeUpdate && "function" == typeof i.callbacks.onBeforeUpdate && i.callbacks.onBeforeUpdate.call(this), t.hasClass(d[3]) && t.removeClass(d[3]), t.hasClass(d[4]) && t.removeClass(d[4]), l.css("max-height", "none"), l.height() !== t.height() && l.css("max-height", t.height()), _.call(this), "y" === i.axis || i.advanced.autoExpandHorizontalScroll || r.css("width", x(r)), n.overflowed = y.call(this), M.call(this), i.autoDraggerLength && S.call(this), b.call(this), T.call(this);
                            var c = [Math.abs(r[0].offsetTop), Math.abs(r[0].offsetLeft)];
                            "x" !== i.axis && (n.overflowed[0] ? s[0].height() > s[0].parent().height() ? B.call(this) : (G(t, c[0].toString(), { dir: "y", dur: 0, overwrite: "none" }), n.contentReset.y = null) : (B.call(this), "y" === i.axis ? k.call(this) : "yx" === i.axis && n.overflowed[1] && G(t, c[1].toString(), { dir: "x", dur: 0, overwrite: "none" }))), "y" !== i.axis && (n.overflowed[1] ? s[1].width() > s[1].parent().width() ? B.call(this) : (G(t, c[1].toString(), { dir: "x", dur: 0, overwrite: "none" }), n.contentReset.x = null) : (B.call(this), "x" === i.axis ? k.call(this) : "yx" === i.axis && n.overflowed[0] && G(t, c[0].toString(), { dir: "y", dur: 0, overwrite: "none" }))), o && n && (2 === o && i.callbacks.onImageLoad && "function" == typeof i.callbacks.onImageLoad ? i.callbacks.onImageLoad.call(this) : 3 === o && i.callbacks.onSelectorChange && "function" == typeof i.callbacks.onSelectorChange ? i.callbacks.onSelectorChange.call(this) : i.callbacks.onUpdate && "function" == typeof i.callbacks.onUpdate && i.callbacks.onUpdate.call(this)), N.call(this)
                        }
                    })
                },
                scrollTo: function(t, o) {
                    if ("undefined" != typeof t && null != t) {
                        var n = f.call(this);
                        return e(n).each(function() {
                            var n = e(this);
                            if (n.data(a)) {
                                var i = n.data(a),
                                    r = i.opt,
                                    l = { trigger: "external", scrollInertia: r.scrollInertia, scrollEasing: "mcsEaseInOut", moveDragger: !1, timeout: 60, callbacks: !0, onStart: !0, onUpdate: !0, onComplete: !0 },
                                    s = e.extend(!0, {}, l, o),
                                    c = Y.call(this, t),
                                    d = s.scrollInertia > 0 && s.scrollInertia < 17 ? 17 : s.scrollInertia;
                                c[0] = X.call(this, c[0], "y"), c[1] = X.call(this, c[1], "x"), s.moveDragger && (c[0] *= i.scrollRatio.y, c[1] *= i.scrollRatio.x), s.dur = ne() ? 0 : d, setTimeout(function() { null !== c[0] && "undefined" != typeof c[0] && "x" !== r.axis && i.overflowed[0] && (s.dir = "y", s.overwrite = "all", G(n, c[0].toString(), s)), null !== c[1] && "undefined" != typeof c[1] && "y" !== r.axis && i.overflowed[1] && (s.dir = "x", s.overwrite = "none", G(n, c[1].toString(), s)) }, s.timeout)
                            }
                        })
                    }
                },
                stop: function() {
                    var t = f.call(this);
                    return e(t).each(function() {
                        var t = e(this);
                        t.data(a) && Q(t)
                    })
                },
                disable: function(t) {
                    var o = f.call(this);
                    return e(o).each(function() {
                        var o = e(this);
                        if (o.data(a)) {
                            o.data(a);
                            N.call(this, "remove"), k.call(this), t && B.call(this), M.call(this, !0), o.addClass(d[3])
                        }
                    })
                },
                destroy: function() {
                    var t = f.call(this);
                    return e(t).each(function() {
                        var n = e(this);
                        if (n.data(a)) {
                            var i = n.data(a),
                                r = i.opt,
                                l = e("#mCSB_" + i.idx),
                                s = e("#mCSB_" + i.idx + "_container"),
                                c = e(".mCSB_" + i.idx + "_scrollbar");
                            r.live && m(r.liveSelector || e(t).selector), N.call(this, "remove"), k.call(this), B.call(this), n.removeData(a), $(this, "mcs"), c.remove(), s.find("img." + d[2]).removeClass(d[2]), l.replaceWith(s.contents()), n.removeClass(o + " _" + a + "_" + i.idx + " " + d[6] + " " + d[7] + " " + d[5] + " " + d[3]).addClass(d[4])
                        }
                    })
                }
            },
            f = function() { return "object" != typeof e(this) || e(this).length < 1 ? n : this },
            h = function(t) {
                var o = ["rounded", "rounded-dark", "rounded-dots", "rounded-dots-dark"],
                    a = ["rounded-dots", "rounded-dots-dark", "3d", "3d-dark", "3d-thick", "3d-thick-dark", "inset", "inset-dark", "inset-2", "inset-2-dark", "inset-3", "inset-3-dark"],
                    n = ["minimal", "minimal-dark"],
                    i = ["minimal", "minimal-dark"],
                    r = ["minimal", "minimal-dark"];
                t.autoDraggerLength = e.inArray(t.theme, o) > -1 ? !1 : t.autoDraggerLength, t.autoExpandScrollbar = e.inArray(t.theme, a) > -1 ? !1 : t.autoExpandScrollbar, t.scrollButtons.enable = e.inArray(t.theme, n) > -1 ? !1 : t.scrollButtons.enable, t.autoHideScrollbar = e.inArray(t.theme, i) > -1 ? !0 : t.autoHideScrollbar, t.scrollbarPosition = e.inArray(t.theme, r) > -1 ? "outside" : t.scrollbarPosition
            },
            m = function(e) { l[e] && (clearTimeout(l[e]), $(l, e)) },
            p = function(e) { return "yx" === e || "xy" === e || "auto" === e ? "yx" : "x" === e || "horizontal" === e ? "x" : "y" },
            g = function(e) { return "stepped" === e || "pixels" === e || "step" === e || "click" === e ? "stepped" : "stepless" },
            v = function() {
                var t = e(this),
                    n = t.data(a),
                    i = n.opt,
                    r = i.autoExpandScrollbar ? " " + d[1] + "_expand" : "",
                    l = ["<div id='mCSB_" + n.idx + "_scrollbar_vertical' class='mCSB_scrollTools mCSB_" + n.idx + "_scrollbar mCS-" + i.theme + " mCSB_scrollTools_vertical" + r + "'><div class='" + d[12] + "'><div id='mCSB_" + n.idx + "_dragger_vertical' class='mCSB_dragger' style='position:absolute;'><div class='mCSB_dragger_bar' /></div><div class='mCSB_draggerRail' /></div></div>", "<div id='mCSB_" + n.idx + "_scrollbar_horizontal' class='mCSB_scrollTools mCSB_" + n.idx + "_scrollbar mCS-" + i.theme + " mCSB_scrollTools_horizontal" + r + "'><div class='" + d[12] + "'><div id='mCSB_" + n.idx + "_dragger_horizontal' class='mCSB_dragger' style='position:absolute;'><div class='mCSB_dragger_bar' /></div><div class='mCSB_draggerRail' /></div></div>"],
                    s = "yx" === i.axis ? "mCSB_vertical_horizontal" : "x" === i.axis ? "mCSB_horizontal" : "mCSB_vertical",
                    c = "yx" === i.axis ? l[0] + l[1] : "x" === i.axis ? l[1] : l[0],
                    u = "yx" === i.axis ? "<div id='mCSB_" + n.idx + "_container_wrapper' class='mCSB_container_wrapper' />" : "",
                    f = i.autoHideScrollbar ? " " + d[6] : "",
                    h = "x" !== i.axis && "rtl" === n.langDir ? " " + d[7] : "";
                i.setWidth && t.css("width", i.setWidth), i.setHeight && t.css("height", i.setHeight), i.setLeft = "y" !== i.axis && "rtl" === n.langDir ? "989999px" : i.setLeft, t.addClass(o + " _" + a + "_" + n.idx + f + h).wrapInner("<div id='mCSB_" + n.idx + "' class='mCustomScrollBox mCS-" + i.theme + " " + s + "'><div id='mCSB_" + n.idx + "_container' class='mCSB_container' style='position:relative; top:" + i.setTop + "; left:" + i.setLeft + ";' dir='" + n.langDir + "' /></div>");
                var m = e("#mCSB_" + n.idx),
                    p = e("#mCSB_" + n.idx + "_container");
                "y" === i.axis || i.advanced.autoExpandHorizontalScroll || p.css("width", x(p)), "outside" === i.scrollbarPosition ? ("static" === t.css("position") && t.css("position", "relative"), t.css("overflow", "visible"), m.addClass("mCSB_outside").after(c)) : (m.addClass("mCSB_inside").append(c), p.wrap(u)), w.call(this);
                var g = [e("#mCSB_" + n.idx + "_dragger_vertical"), e("#mCSB_" + n.idx + "_dragger_horizontal")];
                g[0].css("min-height", g[0].height()), g[1].css("min-width", g[1].width())
            },
            x = function(t) {
                var o = [t[0].scrollWidth, Math.max.apply(Math, t.children().map(function() { return e(this).outerWidth(!0) }).get())],
                    a = t.parent().width();
                return o[0] > a ? o[0] : o[1] > a ? o[1] : "100%"
            },
            _ = function() {
                var t = e(this),
                    o = t.data(a),
                    n = o.opt,
                    i = e("#mCSB_" + o.idx + "_container");
                if (n.advanced.autoExpandHorizontalScroll && "y" !== n.axis) {
                    i.css({ width: "auto", "min-width": 0, "overflow-x": "scroll" });
                    var r = Math.ceil(i[0].scrollWidth);
                    3 === n.advanced.autoExpandHorizontalScroll || 2 !== n.advanced.autoExpandHorizontalScroll && r > i.parent().width() ? i.css({ width: r, "min-width": "100%", "overflow-x": "inherit" }) : i.css({ "overflow-x": "inherit", position: "absolute" }).wrap("<div class='mCSB_h_wrapper' style='position:relative; left:0; width:999999px;' />").css({ width: Math.ceil(i[0].getBoundingClientRect().right + .4) - Math.floor(i[0].getBoundingClientRect().left), "min-width": "100%", position: "relative" }).unwrap()
                }
            },
            w = function() {
                var t = e(this),
                    o = t.data(a),
                    n = o.opt,
                    i = e(".mCSB_" + o.idx + "_scrollbar:first"),
                    r = oe(n.scrollButtons.tabindex) ? "tabindex='" + n.scrollButtons.tabindex + "'" : "",
                    l = ["<a href='#' class='" + d[13] + "' " + r + " />", "<a href='#' class='" + d[14] + "' " + r + " />", "<a href='#' class='" + d[15] + "' " + r + " />", "<a href='#' class='" + d[16] + "' " + r + " />"],
                    s = ["x" === n.axis ? l[2] : l[0], "x" === n.axis ? l[3] : l[1], l[2], l[3]];
                n.scrollButtons.enable && i.prepend(s[0]).append(s[1]).next(".mCSB_scrollTools").prepend(s[2]).append(s[3])
            },
            S = function() {
                var t = e(this),
                    o = t.data(a),
                    n = e("#mCSB_" + o.idx),
                    i = e("#mCSB_" + o.idx + "_container"),
                    r = [e("#mCSB_" + o.idx + "_dragger_vertical"), e("#mCSB_" + o.idx + "_dragger_horizontal")],
                    l = [n.height() / i.outerHeight(!1), n.width() / i.outerWidth(!1)],
                    c = [parseInt(r[0].css("min-height")), Math.round(l[0] * r[0].parent().height()), parseInt(r[1].css("min-width")), Math.round(l[1] * r[1].parent().width())],
                    d = s && c[1] < c[0] ? c[0] : c[1],
                    u = s && c[3] < c[2] ? c[2] : c[3];
                r[0].css({ height: d, "max-height": r[0].parent().height() - 10 }).find(".mCSB_dragger_bar").css({ "line-height": c[0] + "px" }), r[1].css({ width: u, "max-width": r[1].parent().width() - 10 })
            },
            b = function() {
                var t = e(this),
                    o = t.data(a),
                    n = e("#mCSB_" + o.idx),
                    i = e("#mCSB_" + o.idx + "_container"),
                    r = [e("#mCSB_" + o.idx + "_dragger_vertical"), e("#mCSB_" + o.idx + "_dragger_horizontal")],
                    l = [i.outerHeight(!1) - n.height(), i.outerWidth(!1) - n.width()],
                    s = [l[0] / (r[0].parent().height() - r[0].height()), l[1] / (r[1].parent().width() - r[1].width())];
                o.scrollRatio = { y: s[0], x: s[1] }
            },
            C = function(e, t, o) {
                var a = o ? d[0] + "_expanded" : "",
                    n = e.closest(".mCSB_scrollTools");
                "active" === t ? (e.toggleClass(d[0] + " " + a), n.toggleClass(d[1]), e[0]._draggable = e[0]._draggable ? 0 : 1) : e[0]._draggable || ("hide" === t ? (e.removeClass(d[0]), n.removeClass(d[1])) : (e.addClass(d[0]), n.addClass(d[1])))
            },
            y = function() {
                var t = e(this),
                    o = t.data(a),
                    n = e("#mCSB_" + o.idx),
                    i = e("#mCSB_" + o.idx + "_container"),
                    r = null == o.overflowed ? i.height() : i.outerHeight(!1),
                    l = null == o.overflowed ? i.width() : i.outerWidth(!1),
                    s = i[0].scrollHeight,
                    c = i[0].scrollWidth;
                return s > r && (r = s), c > l && (l = c), [r > n.height(), l > n.width()]
            },
            B = function() {
                var t = e(this),
                    o = t.data(a),
                    n = o.opt,
                    i = e("#mCSB_" + o.idx),
                    r = e("#mCSB_" + o.idx + "_container"),
                    l = [e("#mCSB_" + o.idx + "_dragger_vertical"), e("#mCSB_" + o.idx + "_dragger_horizontal")];
                if (Q(t), ("x" !== n.axis && !o.overflowed[0] || "y" === n.axis && o.overflowed[0]) && (l[0].add(r).css("top", 0), G(t, "_resetY")), "y" !== n.axis && !o.overflowed[1] || "x" === n.axis && o.overflowed[1]) { var s = dx = 0; "rtl" === o.langDir && (s = i.width() - r.outerWidth(!1), dx = Math.abs(s / o.scrollRatio.x)), r.css("left", s), l[1].css("left", dx), G(t, "_resetX") }
            },
            T = function() {
                function t() { r = setTimeout(function() { e.event.special.mousewheel ? (clearTimeout(r), W.call(o[0])) : t() }, 100) }
                var o = e(this),
                    n = o.data(a),
                    i = n.opt;
                if (!n.bindEvents) {
                    if (I.call(this), i.contentTouchScroll && D.call(this), E.call(this), i.mouseWheel.enable) {
                        var r;
                        t()
                    }
                    P.call(this), U.call(this), i.advanced.autoScrollOnFocus && H.call(this), i.scrollButtons.enable && F.call(this), i.keyboard.enable && q.call(this), n.bindEvents = !0
                }
            },
            k = function() {
                var t = e(this),
                    o = t.data(a),
                    n = o.opt,
                    i = a + "_" + o.idx,
                    r = ".mCSB_" + o.idx + "_scrollbar",
                    l = e("#mCSB_" + o.idx + ",#mCSB_" + o.idx + "_container,#mCSB_" + o.idx + "_container_wrapper," + r + " ." + d[12] + ",#mCSB_" + o.idx + "_dragger_vertical,#mCSB_" + o.idx + "_dragger_horizontal," + r + ">a"),
                    s = e("#mCSB_" + o.idx + "_container");
                n.advanced.releaseDraggableSelectors && l.add(e(n.advanced.releaseDraggableSelectors)), n.advanced.extraDraggableSelectors && l.add(e(n.advanced.extraDraggableSelectors)), o.bindEvents && (e(document).add(e(!A() || top.document)).unbind("." + i), l.each(function() { e(this).unbind("." + i) }), clearTimeout(t[0]._focusTimeout), $(t[0], "_focusTimeout"), clearTimeout(o.sequential.step), $(o.sequential, "step"), clearTimeout(s[0].onCompleteTimeout), $(s[0], "onCompleteTimeout"), o.bindEvents = !1)
            },
            M = function(t) {
                var o = e(this),
                    n = o.data(a),
                    i = n.opt,
                    r = e("#mCSB_" + n.idx + "_container_wrapper"),
                    l = r.length ? r : e("#mCSB_" + n.idx + "_container"),
                    s = [e("#mCSB_" + n.idx + "_scrollbar_vertical"), e("#mCSB_" + n.idx + "_scrollbar_horizontal")],
                    c = [s[0].find(".mCSB_dragger"), s[1].find(".mCSB_dragger")];
                "x" !== i.axis && (n.overflowed[0] && !t ? (s[0].add(c[0]).add(s[0].children("a")).css("display", "block"), l.removeClass(d[8] + " " + d[10])) : (i.alwaysShowScrollbar ? (2 !== i.alwaysShowScrollbar && c[0].css("display", "none"), l.removeClass(d[10])) : (s[0].css("display", "none"), l.addClass(d[10])), l.addClass(d[8]))), "y" !== i.axis && (n.overflowed[1] && !t ? (s[1].add(c[1]).add(s[1].children("a")).css("display", "block"), l.removeClass(d[9] + " " + d[11])) : (i.alwaysShowScrollbar ? (2 !== i.alwaysShowScrollbar && c[1].css("display", "none"), l.removeClass(d[11])) : (s[1].css("display", "none"), l.addClass(d[11])), l.addClass(d[9]))), n.overflowed[0] || n.overflowed[1] ? o.removeClass(d[5]) : o.addClass(d[5])
            },
            O = function(t) {
                var o = t.type,
                    a = t.target.ownerDocument !== document && null !== frameElement ? [e(frameElement).offset().top, e(frameElement).offset().left] : null,
                    n = A() && t.target.ownerDocument !== top.document && null !== frameElement ? [e(t.view.frameElement).offset().top, e(t.view.frameElement).offset().left] : [0, 0];
                switch (o) {
                    case "pointerdown":
                    case "MSPointerDown":
                    case "pointermove":
                    case "MSPointerMove":
                    case "pointerup":
                    case "MSPointerUp":
                        return a ? [t.originalEvent.pageY - a[0] + n[0], t.originalEvent.pageX - a[1] + n[1], !1] : [t.originalEvent.pageY, t.originalEvent.pageX, !1];
                    case "touchstart":
                    case "touchmove":
                    case "touchend":
                        var i = t.originalEvent.touches[0] || t.originalEvent.changedTouches[0],
                            r = t.originalEvent.touches.length || t.originalEvent.changedTouches.length;
                        return t.target.ownerDocument !== document ? [i.screenY, i.screenX, r > 1] : [i.pageY, i.pageX, r > 1];
                    default:
                        return a ? [t.pageY - a[0] + n[0], t.pageX - a[1] + n[1], !1] : [t.pageY, t.pageX, !1]
                }
            },
            I = function() {
                function t(e, t, a, n) {
                    if (h[0].idleTimer = d.scrollInertia < 233 ? 250 : 0, o.attr("id") === f[1]) var i = "x",
                        s = (o[0].offsetLeft - t + n) * l.scrollRatio.x;
                    else var i = "y",
                        s = (o[0].offsetTop - e + a) * l.scrollRatio.y;
                    G(r, s.toString(), { dir: i, drag: !0 })
                }
                var o, n, i, r = e(this),
                    l = r.data(a),
                    d = l.opt,
                    u = a + "_" + l.idx,
                    f = ["mCSB_" + l.idx + "_dragger_vertical", "mCSB_" + l.idx + "_dragger_horizontal"],
                    h = e("#mCSB_" + l.idx + "_container"),
                    m = e("#" + f[0] + ",#" + f[1]),
                    p = d.advanced.releaseDraggableSelectors ? m.add(e(d.advanced.releaseDraggableSelectors)) : m,
                    g = d.advanced.extraDraggableSelectors ? e(!A() || top.document).add(e(d.advanced.extraDraggableSelectors)) : e(!A() || top.document);
                m.bind("contextmenu." + u, function(e) { e.preventDefault() }).bind("mousedown." + u + " touchstart." + u + " pointerdown." + u + " MSPointerDown." + u, function(t) {
                    if (t.stopImmediatePropagation(), t.preventDefault(), ee(t)) {
                        c = !0, s && (document.onselectstart = function() { return !1 }), L.call(h, !1), Q(r), o = e(this);
                        var a = o.offset(),
                            l = O(t)[0] - a.top,
                            u = O(t)[1] - a.left,
                            f = o.height() + a.top,
                            m = o.width() + a.left;
                        f > l && l > 0 && m > u && u > 0 && (n = l, i = u), C(o, "active", d.autoExpandScrollbar)
                    }
                }).bind("touchmove." + u, function(e) {
                    e.stopImmediatePropagation(), e.preventDefault();
                    var a = o.offset(),
                        r = O(e)[0] - a.top,
                        l = O(e)[1] - a.left;
                    t(n, i, r, l)
                }), e(document).add(g).bind("mousemove." + u + " pointermove." + u + " MSPointerMove." + u, function(e) {
                    if (o) {
                        var a = o.offset(),
                            r = O(e)[0] - a.top,
                            l = O(e)[1] - a.left;
                        if (n === r && i === l) return;
                        t(n, i, r, l)
                    }
                }).add(p).bind("mouseup." + u + " touchend." + u + " pointerup." + u + " MSPointerUp." + u, function() { o && (C(o, "active", d.autoExpandScrollbar), o = null), c = !1, s && (document.onselectstart = null), L.call(h, !0) })
            },
            D = function() {
                function o(e) {
                    if (!te(e) || c || O(e)[2]) return void(t = 0);
                    t = 1, b = 0, C = 0, d = 1, y.removeClass("mCS_touch_action");
                    var o = I.offset();
                    u = O(e)[0] - o.top, f = O(e)[1] - o.left, z = [O(e)[0], O(e)[1]]
                }

                function n(e) {
                    if (te(e) && !c && !O(e)[2] && (T.documentTouchScroll || e.preventDefault(), e.stopImmediatePropagation(), (!C || b) && d)) {
                        g = K();
                        var t = M.offset(),
                            o = O(e)[0] - t.top,
                            a = O(e)[1] - t.left,
                            n = "mcsLinearOut";
                        if (E.push(o), W.push(a), z[2] = Math.abs(O(e)[0] - z[0]), z[3] = Math.abs(O(e)[1] - z[1]), B.overflowed[0]) var i = D[0].parent().height() - D[0].height(),
                            r = u - o > 0 && o - u > -(i * B.scrollRatio.y) && (2 * z[3] < z[2] || "yx" === T.axis);
                        if (B.overflowed[1]) var l = D[1].parent().width() - D[1].width(),
                            h = f - a > 0 && a - f > -(l * B.scrollRatio.x) && (2 * z[2] < z[3] || "yx" === T.axis);
                        r || h ? (U || e.preventDefault(), b = 1) : (C = 1, y.addClass("mCS_touch_action")), U && e.preventDefault(), w = "yx" === T.axis ? [u - o, f - a] : "x" === T.axis ? [null, f - a] : [u - o, null], I[0].idleTimer = 250, B.overflowed[0] && s(w[0], R, n, "y", "all", !0), B.overflowed[1] && s(w[1], R, n, "x", L, !0)
                    }
                }

                function i(e) {
                    if (!te(e) || c || O(e)[2]) return void(t = 0);
                    t = 1, e.stopImmediatePropagation(), Q(y), p = K();
                    var o = M.offset();
                    h = O(e)[0] - o.top, m = O(e)[1] - o.left, E = [], W = []
                }

                function r(e) {
                    if (te(e) && !c && !O(e)[2]) {
                        d = 0, e.stopImmediatePropagation(), b = 0, C = 0, v = K();
                        var t = M.offset(),
                            o = O(e)[0] - t.top,
                            a = O(e)[1] - t.left;
                        if (!(v - g > 30)) {
                            _ = 1e3 / (v - p);
                            var n = "mcsEaseOut",
                                i = 2.5 > _,
                                r = i ? [E[E.length - 2], W[W.length - 2]] : [0, 0];
                            x = i ? [o - r[0], a - r[1]] : [o - h, a - m];
                            var u = [Math.abs(x[0]), Math.abs(x[1])];
                            _ = i ? [Math.abs(x[0] / 4), Math.abs(x[1] / 4)] : [_, _];
                            var f = [Math.abs(I[0].offsetTop) - x[0] * l(u[0] / _[0], _[0]), Math.abs(I[0].offsetLeft) - x[1] * l(u[1] / _[1], _[1])];
                            w = "yx" === T.axis ? [f[0], f[1]] : "x" === T.axis ? [null, f[1]] : [f[0], null], S = [4 * u[0] + T.scrollInertia, 4 * u[1] + T.scrollInertia];
                            var y = parseInt(T.contentTouchScroll) || 0;
                            w[0] = u[0] > y ? w[0] : 0, w[1] = u[1] > y ? w[1] : 0, B.overflowed[0] && s(w[0], S[0], n, "y", L, !1), B.overflowed[1] && s(w[1], S[1], n, "x", L, !1)
                        }
                    }
                }

                function l(e, t) { var o = [1.5 * t, 2 * t, t / 1.5, t / 2]; return e > 90 ? t > 4 ? o[0] : o[3] : e > 60 ? t > 3 ? o[3] : o[2] : e > 30 ? t > 8 ? o[1] : t > 6 ? o[0] : t > 4 ? t : o[2] : t > 8 ? t : o[3] }

                function s(e, t, o, a, n, i) { e && G(y, e.toString(), { dur: t, scrollEasing: o, dir: a, overwrite: n, drag: i }) }
                var d, u, f, h, m, p, g, v, x, _, w, S, b, C, y = e(this),
                    B = y.data(a),
                    T = B.opt,
                    k = a + "_" + B.idx,
                    M = e("#mCSB_" + B.idx),
                    I = e("#mCSB_" + B.idx + "_container"),
                    D = [e("#mCSB_" + B.idx + "_dragger_vertical"), e("#mCSB_" + B.idx + "_dragger_horizontal")],
                    E = [],
                    W = [],
                    R = 0,
                    L = "yx" === T.axis ? "none" : "all",
                    z = [],
                    P = I.find("iframe"),
                    H = ["touchstart." + k + " pointerdown." + k + " MSPointerDown." + k, "touchmove." + k + " pointermove." + k + " MSPointerMove." + k, "touchend." + k + " pointerup." + k + " MSPointerUp." + k],
                    U = void 0 !== document.body.style.touchAction && "" !== document.body.style.touchAction;
                I.bind(H[0], function(e) { o(e) }).bind(H[1], function(e) { n(e) }), M.bind(H[0], function(e) { i(e) }).bind(H[2], function(e) { r(e) }), P.length && P.each(function() { e(this).bind("load", function() { A(this) && e(this.contentDocument || this.contentWindow.document).bind(H[0], function(e) { o(e), i(e) }).bind(H[1], function(e) { n(e) }).bind(H[2], function(e) { r(e) }) }) })
            },
            E = function() {
                function o() { return window.getSelection ? window.getSelection().toString() : document.selection && "Control" != document.selection.type ? document.selection.createRange().text : 0 }

                function n(e, t, o) { d.type = o && i ? "stepped" : "stepless", d.scrollAmount = 10, j(r, e, t, "mcsLinearOut", o ? 60 : null) }
                var i, r = e(this),
                    l = r.data(a),
                    s = l.opt,
                    d = l.sequential,
                    u = a + "_" + l.idx,
                    f = e("#mCSB_" + l.idx + "_container"),
                    h = f.parent();
                f.bind("mousedown." + u, function() { t || i || (i = 1, c = !0) }).add(document).bind("mousemove." + u, function(e) {
                    if (!t && i && o()) {
                        var a = f.offset(),
                            r = O(e)[0] - a.top + f[0].offsetTop,
                            c = O(e)[1] - a.left + f[0].offsetLeft;
                        r > 0 && r < h.height() && c > 0 && c < h.width() ? d.step && n("off", null, "stepped") : ("x" !== s.axis && l.overflowed[0] && (0 > r ? n("on", 38) : r > h.height() && n("on", 40)), "y" !== s.axis && l.overflowed[1] && (0 > c ? n("on", 37) : c > h.width() && n("on", 39)))
                    }
                }).bind("mouseup." + u + " dragend." + u, function() { t || (i && (i = 0, n("off", null)), c = !1) })
            },
            W = function() {
                function t(t, a) {
                    if (Q(o), !z(o, t.target)) {
                        var r = "auto" !== i.mouseWheel.deltaFactor ? parseInt(i.mouseWheel.deltaFactor) : s && t.deltaFactor < 100 ? 100 : t.deltaFactor || 100,
                            d = i.scrollInertia;
                        if ("x" === i.axis || "x" === i.mouseWheel.axis) var u = "x",
                            f = [Math.round(r * n.scrollRatio.x), parseInt(i.mouseWheel.scrollAmount)],
                            h = "auto" !== i.mouseWheel.scrollAmount ? f[1] : f[0] >= l.width() ? .9 * l.width() : f[0],
                            m = Math.abs(e("#mCSB_" + n.idx + "_container")[0].offsetLeft),
                            p = c[1][0].offsetLeft,
                            g = c[1].parent().width() - c[1].width(),
                            v = "y" === i.mouseWheel.axis ? t.deltaY || a : t.deltaX;
                        else var u = "y",
                            f = [Math.round(r * n.scrollRatio.y), parseInt(i.mouseWheel.scrollAmount)],
                            h = "auto" !== i.mouseWheel.scrollAmount ? f[1] : f[0] >= l.height() ? .9 * l.height() : f[0],
                            m = Math.abs(e("#mCSB_" + n.idx + "_container")[0].offsetTop),
                            p = c[0][0].offsetTop,
                            g = c[0].parent().height() - c[0].height(),
                            v = t.deltaY || a;
                        "y" === u && !n.overflowed[0] || "x" === u && !n.overflowed[1] || ((i.mouseWheel.invert || t.webkitDirectionInvertedFromDevice) && (v = -v), i.mouseWheel.normalizeDelta && (v = 0 > v ? -1 : 1), (v > 0 && 0 !== p || 0 > v && p !== g || i.mouseWheel.preventDefault) && (t.stopImmediatePropagation(), t.preventDefault()), t.deltaFactor < 5 && !i.mouseWheel.normalizeDelta && (h = t.deltaFactor, d = 17), G(o, (m - v * h).toString(), { dir: u, dur: d }))
                    }
                }
                if (e(this).data(a)) {
                    var o = e(this),
                        n = o.data(a),
                        i = n.opt,
                        r = a + "_" + n.idx,
                        l = e("#mCSB_" + n.idx),
                        c = [e("#mCSB_" + n.idx + "_dragger_vertical"), e("#mCSB_" + n.idx + "_dragger_horizontal")],
                        d = e("#mCSB_" + n.idx + "_container").find("iframe");
                    d.length && d.each(function() { e(this).bind("load", function() { A(this) && e(this.contentDocument || this.contentWindow.document).bind("mousewheel." + r, function(e, o) { t(e, o) }) }) }), l.bind("mousewheel." + r, function(e, o) { t(e, o) })
                }
            },
            R = new Object,
            A = function(t) {
                var o = !1,
                    a = !1,
                    n = null;
                if (void 0 === t ? a = "#empty" : void 0 !== e(t).attr("id") && (a = e(t).attr("id")), a !== !1 && void 0 !== R[a]) return R[a];
                if (t) {
                    try {
                        var i = t.contentDocument || t.contentWindow.document;
                        n = i.body.innerHTML
                    } catch (r) {}
                    o = null !== n
                } else {
                    try {
                        var i = top.document;
                        n = i.body.innerHTML
                    } catch (r) {}
                    o = null !== n
                }
                return a !== !1 && (R[a] = o), o
            },
            L = function(e) {
                var t = this.find("iframe");
                if (t.length) {
                    var o = e ? "auto" : "none";
                    t.css("pointer-events", o)
                }
            },
            z = function(t, o) {
                var n = o.nodeName.toLowerCase(),
                    i = t.data(a).opt.mouseWheel.disableOver,
                    r = ["select", "textarea"];
                return e.inArray(n, i) > -1 && !(e.inArray(n, r) > -1 && !e(o).is(":focus"))
            },
            P = function() {
                var t, o = e(this),
                    n = o.data(a),
                    i = a + "_" + n.idx,
                    r = e("#mCSB_" + n.idx + "_container"),
                    l = r.parent(),
                    s = e(".mCSB_" + n.idx + "_scrollbar ." + d[12]);
                s.bind("mousedown." + i + " touchstart." + i + " pointerdown." + i + " MSPointerDown." + i, function(o) { c = !0, e(o.target).hasClass("mCSB_dragger") || (t = 1) }).bind("touchend." + i + " pointerup." + i + " MSPointerUp." + i, function() { c = !1 }).bind("click." + i, function(a) {
                    if (t && (t = 0, e(a.target).hasClass(d[12]) || e(a.target).hasClass("mCSB_draggerRail"))) {
                        Q(o);
                        var i = e(this),
                            s = i.find(".mCSB_dragger");
                        if (i.parent(".mCSB_scrollTools_horizontal").length > 0) {
                            if (!n.overflowed[1]) return;
                            var c = "x",
                                u = a.pageX > s.offset().left ? -1 : 1,
                                f = Math.abs(r[0].offsetLeft) - u * (.9 * l.width())
                        } else {
                            if (!n.overflowed[0]) return;
                            var c = "y",
                                u = a.pageY > s.offset().top ? -1 : 1,
                                f = Math.abs(r[0].offsetTop) - u * (.9 * l.height())
                        }
                        G(o, f.toString(), { dir: c, scrollEasing: "mcsEaseInOut" })
                    }
                })
            },
            H = function() {
                var t = e(this),
                    o = t.data(a),
                    n = o.opt,
                    i = a + "_" + o.idx,
                    r = e("#mCSB_" + o.idx + "_container"),
                    l = r.parent();
                r.bind("focusin." + i, function() {
                    var o = e(document.activeElement),
                        a = r.find(".mCustomScrollBox").length,
                        i = 0;
                    o.is(n.advanced.autoScrollOnFocus) && (Q(t), clearTimeout(t[0]._focusTimeout), t[0]._focusTimer = a ? (i + 17) * a : 0, t[0]._focusTimeout = setTimeout(function() {
                        var e = [ae(o)[0], ae(o)[1]],
                            a = [r[0].offsetTop, r[0].offsetLeft],
                            s = [a[0] + e[0] >= 0 && a[0] + e[0] < l.height() - o.outerHeight(!1), a[1] + e[1] >= 0 && a[0] + e[1] < l.width() - o.outerWidth(!1)],
                            c = "yx" !== n.axis || s[0] || s[1] ? "all" : "none";
                        "x" === n.axis || s[0] || G(t, e[0].toString(), { dir: "y", scrollEasing: "mcsEaseInOut", overwrite: c, dur: i }), "y" === n.axis || s[1] || G(t, e[1].toString(), { dir: "x", scrollEasing: "mcsEaseInOut", overwrite: c, dur: i })
                    }, t[0]._focusTimer))
                })
            },
            U = function() {
                var t = e(this),
                    o = t.data(a),
                    n = a + "_" + o.idx,
                    i = e("#mCSB_" + o.idx + "_container").parent();
                i.bind("scroll." + n, function() { 0 === i.scrollTop() && 0 === i.scrollLeft() || e(".mCSB_" + o.idx + "_scrollbar").css("visibility", "hidden") })
            },
            F = function() {
                var t = e(this),
                    o = t.data(a),
                    n = o.opt,
                    i = o.sequential,
                    r = a + "_" + o.idx,
                    l = ".mCSB_" + o.idx + "_scrollbar",
                    s = e(l + ">a");
                s.bind("contextmenu." + r, function(e) { e.preventDefault() }).bind("mousedown." + r + " touchstart." + r + " pointerdown." + r + " MSPointerDown." + r + " mouseup." + r + " touchend." + r + " pointerup." + r + " MSPointerUp." + r + " mouseout." + r + " pointerout." + r + " MSPointerOut." + r + " click." + r, function(a) {
                    function r(e, o) { i.scrollAmount = n.scrollButtons.scrollAmount, j(t, e, o) }
                    if (a.preventDefault(), ee(a)) {
                        var l = e(this).attr("class");
                        switch (i.type = n.scrollButtons.scrollType, a.type) {
                            case "mousedown":
                            case "touchstart":
                            case "pointerdown":
                            case "MSPointerDown":
                                if ("stepped" === i.type) return;
                                c = !0, o.tweenRunning = !1, r("on", l);
                                break;
                            case "mouseup":
                            case "touchend":
                            case "pointerup":
                            case "MSPointerUp":
                            case "mouseout":
                            case "pointerout":
                            case "MSPointerOut":
                                if ("stepped" === i.type) return;
                                c = !1, i.dir && r("off", l);
                                break;
                            case "click":
                                if ("stepped" !== i.type || o.tweenRunning) return;
                                r("on", l)
                        }
                    }
                })
            },
            q = function() {
                function t(t) {
                    function a(e, t) { r.type = i.keyboard.scrollType, r.scrollAmount = i.keyboard.scrollAmount, "stepped" === r.type && n.tweenRunning || j(o, e, t) }
                    switch (t.type) {
                        case "blur":
                            n.tweenRunning && r.dir && a("off", null);
                            break;
                        case "keydown":
                        case "keyup":
                            var l = t.keyCode ? t.keyCode : t.which,
                                s = "on";
                            if ("x" !== i.axis && (38 === l || 40 === l) || "y" !== i.axis && (37 === l || 39 === l)) { if ((38 === l || 40 === l) && !n.overflowed[0] || (37 === l || 39 === l) && !n.overflowed[1]) return; "keyup" === t.type && (s = "off"), e(document.activeElement).is(u) || (t.preventDefault(), t.stopImmediatePropagation(), a(s, l)) } else if (33 === l || 34 === l) {
                                if ((n.overflowed[0] || n.overflowed[1]) && (t.preventDefault(), t.stopImmediatePropagation()), "keyup" === t.type) {
                                    Q(o);
                                    var f = 34 === l ? -1 : 1;
                                    if ("x" === i.axis || "yx" === i.axis && n.overflowed[1] && !n.overflowed[0]) var h = "x",
                                        m = Math.abs(c[0].offsetLeft) - f * (.9 * d.width());
                                    else var h = "y",
                                        m = Math.abs(c[0].offsetTop) - f * (.9 * d.height());
                                    G(o, m.toString(), { dir: h, scrollEasing: "mcsEaseInOut" })
                                }
                            } else if ((35 === l || 36 === l) && !e(document.activeElement).is(u) && ((n.overflowed[0] || n.overflowed[1]) && (t.preventDefault(), t.stopImmediatePropagation()), "keyup" === t.type)) {
                                if ("x" === i.axis || "yx" === i.axis && n.overflowed[1] && !n.overflowed[0]) var h = "x",
                                    m = 35 === l ? Math.abs(d.width() - c.outerWidth(!1)) : 0;
                                else var h = "y",
                                    m = 35 === l ? Math.abs(d.height() - c.outerHeight(!1)) : 0;
                                G(o, m.toString(), { dir: h, scrollEasing: "mcsEaseInOut" })
                            }
                    }
                }
                var o = e(this),
                    n = o.data(a),
                    i = n.opt,
                    r = n.sequential,
                    l = a + "_" + n.idx,
                    s = e("#mCSB_" + n.idx),
                    c = e("#mCSB_" + n.idx + "_container"),
                    d = c.parent(),
                    u = "input,textarea,select,datalist,keygen,[contenteditable='true']",
                    f = c.find("iframe"),
                    h = ["blur." + l + " keydown." + l + " keyup." + l];
                f.length && f.each(function() { e(this).bind("load", function() { A(this) && e(this.contentDocument || this.contentWindow.document).bind(h[0], function(e) { t(e) }) }) }), s.attr("tabindex", "0").bind(h[0], function(e) { t(e) })
            },
            j = function(t, o, n, i, r) {
                function l(e) {
                    u.snapAmount && (f.scrollAmount = u.snapAmount instanceof Array ? "x" === f.dir[0] ? u.snapAmount[1] : u.snapAmount[0] : u.snapAmount);
                    var o = "stepped" !== f.type,
                        a = r ? r : e ? o ? p / 1.5 : g : 1e3 / 60,
                        n = e ? o ? 7.5 : 40 : 2.5,
                        s = [Math.abs(h[0].offsetTop), Math.abs(h[0].offsetLeft)],
                        d = [c.scrollRatio.y > 10 ? 10 : c.scrollRatio.y, c.scrollRatio.x > 10 ? 10 : c.scrollRatio.x],
                        m = "x" === f.dir[0] ? s[1] + f.dir[1] * (d[1] * n) : s[0] + f.dir[1] * (d[0] * n),
                        v = "x" === f.dir[0] ? s[1] + f.dir[1] * parseInt(f.scrollAmount) : s[0] + f.dir[1] * parseInt(f.scrollAmount),
                        x = "auto" !== f.scrollAmount ? v : m,
                        _ = i ? i : e ? o ? "mcsLinearOut" : "mcsEaseInOut" : "mcsLinear",
                        w = !!e;
                    return e && 17 > a && (x = "x" === f.dir[0] ? s[1] : s[0]), G(t, x.toString(), { dir: f.dir[0], scrollEasing: _, dur: a, onComplete: w }), e ? void(f.dir = !1) : (clearTimeout(f.step), void(f.step = setTimeout(function() { l() }, a)))
                }

                function s() { clearTimeout(f.step), $(f, "step"), Q(t) }
                var c = t.data(a),
                    u = c.opt,
                    f = c.sequential,
                    h = e("#mCSB_" + c.idx + "_container"),
                    m = "stepped" === f.type,
                    p = u.scrollInertia < 26 ? 26 : u.scrollInertia,
                    g = u.scrollInertia < 1 ? 17 : u.scrollInertia;
                switch (o) {
                    case "on":
                        if (f.dir = [n === d[16] || n === d[15] || 39 === n || 37 === n ? "x" : "y", n === d[13] || n === d[15] || 38 === n || 37 === n ? -1 : 1], Q(t), oe(n) && "stepped" === f.type) return;
                        l(m);
                        break;
                    case "off":
                        s(), (m || c.tweenRunning && f.dir) && l(!0)
                }
            },
            Y = function(t) {
                var o = e(this).data(a).opt,
                    n = [];
                return "function" == typeof t && (t = t()), t instanceof Array ? n = t.length > 1 ? [t[0], t[1]] : "x" === o.axis ? [null, t[0]] : [t[0], null] : (n[0] = t.y ? t.y : t.x || "x" === o.axis ? null : t, n[1] = t.x ? t.x : t.y || "y" === o.axis ? null : t), "function" == typeof n[0] && (n[0] = n[0]()), "function" == typeof n[1] && (n[1] = n[1]()), n
            },
            X = function(t, o) {
                if (null != t && "undefined" != typeof t) {
                    var n = e(this),
                        i = n.data(a),
                        r = i.opt,
                        l = e("#mCSB_" + i.idx + "_container"),
                        s = l.parent(),
                        c = typeof t;
                    o || (o = "x" === r.axis ? "x" : "y");
                    var d = "x" === o ? l.outerWidth(!1) - s.width() : l.outerHeight(!1) - s.height(),
                        f = "x" === o ? l[0].offsetLeft : l[0].offsetTop,
                        h = "x" === o ? "left" : "top";
                    switch (c) {
                        case "function":
                            return t();
                        case "object":
                            var m = t.jquery ? t : e(t);
                            if (!m.length) return;
                            return "x" === o ? ae(m)[1] : ae(m)[0];
                        case "string":
                        case "number":
                            if (oe(t)) return Math.abs(t);
                            if (-1 !== t.indexOf("%")) return Math.abs(d * parseInt(t) / 100);
                            if (-1 !== t.indexOf("-=")) return Math.abs(f - parseInt(t.split("-=")[1]));
                            if (-1 !== t.indexOf("+=")) { var p = f + parseInt(t.split("+=")[1]); return p >= 0 ? 0 : Math.abs(p) }
                            if (-1 !== t.indexOf("px") && oe(t.split("px")[0])) return Math.abs(t.split("px")[0]);
                            if ("top" === t || "left" === t) return 0;
                            if ("bottom" === t) return Math.abs(s.height() - l.outerHeight(!1));
                            if ("right" === t) return Math.abs(s.width() - l.outerWidth(!1));
                            if ("first" === t || "last" === t) { var m = l.find(":" + t); return "x" === o ? ae(m)[1] : ae(m)[0] }
                            return e(t).length ? "x" === o ? ae(e(t))[1] : ae(e(t))[0] : (l.css(h, t), void u.update.call(null, n[0]))
                    }
                }
            },
            N = function(t) {
                function o() { return clearTimeout(f[0].autoUpdate), 0 === l.parents("html").length ? void(l = null) : void(f[0].autoUpdate = setTimeout(function() { return c.advanced.updateOnSelectorChange && (s.poll.change.n = i(), s.poll.change.n !== s.poll.change.o) ? (s.poll.change.o = s.poll.change.n, void r(3)) : c.advanced.updateOnContentResize && (s.poll.size.n = l[0].scrollHeight + l[0].scrollWidth + f[0].offsetHeight + l[0].offsetHeight + l[0].offsetWidth, s.poll.size.n !== s.poll.size.o) ? (s.poll.size.o = s.poll.size.n, void r(1)) : !c.advanced.updateOnImageLoad || "auto" === c.advanced.updateOnImageLoad && "y" === c.axis || (s.poll.img.n = f.find("img").length, s.poll.img.n === s.poll.img.o) ? void((c.advanced.updateOnSelectorChange || c.advanced.updateOnContentResize || c.advanced.updateOnImageLoad) && o()) : (s.poll.img.o = s.poll.img.n, void f.find("img").each(function() { n(this) })) }, c.advanced.autoUpdateTimeout)) }

                function n(t) {
                    function o(e, t) {
                        return function() {
                            return t.apply(e, arguments)
                        }
                    }

                    function a() { this.onload = null, e(t).addClass(d[2]), r(2) }
                    if (e(t).hasClass(d[2])) return void r();
                    var n = new Image;
                    n.onload = o(n, a), n.src = t.src
                }

                function i() {
                    c.advanced.updateOnSelectorChange === !0 && (c.advanced.updateOnSelectorChange = "*");
                    var e = 0,
                        t = f.find(c.advanced.updateOnSelectorChange);
                    return c.advanced.updateOnSelectorChange && t.length > 0 && t.each(function() { e += this.offsetHeight + this.offsetWidth }), e
                }

                function r(e) { clearTimeout(f[0].autoUpdate), u.update.call(null, l[0], e) }
                var l = e(this),
                    s = l.data(a),
                    c = s.opt,
                    f = e("#mCSB_" + s.idx + "_container");
                return t ? (clearTimeout(f[0].autoUpdate), void $(f[0], "autoUpdate")) : void o()
            },
            V = function(e, t, o) { return Math.round(e / t) * t - o },
            Q = function(t) {
                var o = t.data(a),
                    n = e("#mCSB_" + o.idx + "_container,#mCSB_" + o.idx + "_container_wrapper,#mCSB_" + o.idx + "_dragger_vertical,#mCSB_" + o.idx + "_dragger_horizontal");
                n.each(function() { Z.call(this) })
            },
            G = function(t, o, n) {
                function i(e) { return s && c.callbacks[e] && "function" == typeof c.callbacks[e] }

                function r() { return [c.callbacks.alwaysTriggerOffsets || w >= S[0] + y, c.callbacks.alwaysTriggerOffsets || -B >= w] }

                function l() {
                    var e = [h[0].offsetTop, h[0].offsetLeft],
                        o = [x[0].offsetTop, x[0].offsetLeft],
                        a = [h.outerHeight(!1), h.outerWidth(!1)],
                        i = [f.height(), f.width()];
                    t[0].mcs = { content: h, top: e[0], left: e[1], draggerTop: o[0], draggerLeft: o[1], topPct: Math.round(100 * Math.abs(e[0]) / (Math.abs(a[0]) - i[0])), leftPct: Math.round(100 * Math.abs(e[1]) / (Math.abs(a[1]) - i[1])), direction: n.dir }
                }
                var s = t.data(a),
                    c = s.opt,
                    d = { trigger: "internal", dir: "y", scrollEasing: "mcsEaseOut", drag: !1, dur: c.scrollInertia, overwrite: "all", callbacks: !0, onStart: !0, onUpdate: !0, onComplete: !0 },
                    n = e.extend(d, n),
                    u = [n.dur, n.drag ? 0 : n.dur],
                    f = e("#mCSB_" + s.idx),
                    h = e("#mCSB_" + s.idx + "_container"),
                    m = h.parent(),
                    p = c.callbacks.onTotalScrollOffset ? Y.call(t, c.callbacks.onTotalScrollOffset) : [0, 0],
                    g = c.callbacks.onTotalScrollBackOffset ? Y.call(t, c.callbacks.onTotalScrollBackOffset) : [0, 0];
                if (s.trigger = n.trigger, 0 === m.scrollTop() && 0 === m.scrollLeft() || (e(".mCSB_" + s.idx + "_scrollbar").css("visibility", "visible"), m.scrollTop(0).scrollLeft(0)), "_resetY" !== o || s.contentReset.y || (i("onOverflowYNone") && c.callbacks.onOverflowYNone.call(t[0]), s.contentReset.y = 1), "_resetX" !== o || s.contentReset.x || (i("onOverflowXNone") && c.callbacks.onOverflowXNone.call(t[0]), s.contentReset.x = 1), "_resetY" !== o && "_resetX" !== o) {
                    if (!s.contentReset.y && t[0].mcs || !s.overflowed[0] || (i("onOverflowY") && c.callbacks.onOverflowY.call(t[0]), s.contentReset.x = null), !s.contentReset.x && t[0].mcs || !s.overflowed[1] || (i("onOverflowX") && c.callbacks.onOverflowX.call(t[0]), s.contentReset.x = null), c.snapAmount) {
                        var v = c.snapAmount instanceof Array ? "x" === n.dir ? c.snapAmount[1] : c.snapAmount[0] : c.snapAmount;
                        o = V(o, v, c.snapOffset)
                    }
                    switch (n.dir) {
                        case "x":
                            var x = e("#mCSB_" + s.idx + "_dragger_horizontal"),
                                _ = "left",
                                w = h[0].offsetLeft,
                                S = [f.width() - h.outerWidth(!1), x.parent().width() - x.width()],
                                b = [o, 0 === o ? 0 : o / s.scrollRatio.x],
                                y = p[1],
                                B = g[1],
                                T = y > 0 ? y / s.scrollRatio.x : 0,
                                k = B > 0 ? B / s.scrollRatio.x : 0;
                            break;
                        case "y":
                            var x = e("#mCSB_" + s.idx + "_dragger_vertical"),
                                _ = "top",
                                w = h[0].offsetTop,
                                S = [f.height() - h.outerHeight(!1), x.parent().height() - x.height()],
                                b = [o, 0 === o ? 0 : o / s.scrollRatio.y],
                                y = p[0],
                                B = g[0],
                                T = y > 0 ? y / s.scrollRatio.y : 0,
                                k = B > 0 ? B / s.scrollRatio.y : 0
                    }
                    b[1] < 0 || 0 === b[0] && 0 === b[1] ? b = [0, 0] : b[1] >= S[1] ? b = [S[0], S[1]] : b[0] = -b[0], t[0].mcs || (l(), i("onInit") && c.callbacks.onInit.call(t[0])), clearTimeout(h[0].onCompleteTimeout), J(x[0], _, Math.round(b[1]), u[1], n.scrollEasing), !s.tweenRunning && (0 === w && b[0] >= 0 || w === S[0] && b[0] <= S[0]) || J(h[0], _, Math.round(b[0]), u[0], n.scrollEasing, n.overwrite, {
                        onStart: function() { n.callbacks && n.onStart && !s.tweenRunning && (i("onScrollStart") && (l(), c.callbacks.onScrollStart.call(t[0])), s.tweenRunning = !0, C(x), s.cbOffsets = r()) },
                        onUpdate: function() { n.callbacks && n.onUpdate && i("whileScrolling") && (l(), c.callbacks.whileScrolling.call(t[0])) },
                        onComplete: function() {
                            if (n.callbacks && n.onComplete) {
                                "yx" === c.axis && clearTimeout(h[0].onCompleteTimeout);
                                var e = h[0].idleTimer || 0;
                                h[0].onCompleteTimeout = setTimeout(function() { i("onScroll") && (l(), c.callbacks.onScroll.call(t[0])), i("onTotalScroll") && b[1] >= S[1] - T && s.cbOffsets[0] && (l(), c.callbacks.onTotalScroll.call(t[0])), i("onTotalScrollBack") && b[1] <= k && s.cbOffsets[1] && (l(), c.callbacks.onTotalScrollBack.call(t[0])), s.tweenRunning = !1, h[0].idleTimer = 0, C(x, "hide") }, e)
                            }
                        }
                    })
                }
            },
            J = function(e, t, o, a, n, i, r) {
                function l() { S.stop || (x || m.call(), x = K() - v, s(), x >= S.time && (S.time = x > S.time ? x + f - (x - S.time) : x + f - 1, S.time < x + 1 && (S.time = x + 1)), S.time < a ? S.id = h(l) : g.call()) }

                function s() { a > 0 ? (S.currVal = u(S.time, _, b, a, n), w[t] = Math.round(S.currVal) + "px") : w[t] = o + "px", p.call() }

                function c() { f = 1e3 / 60, S.time = x + f, h = window.requestAnimationFrame ? window.requestAnimationFrame : function(e) { return s(), setTimeout(e, .01) }, S.id = h(l) }

                function d() { null != S.id && (window.requestAnimationFrame ? window.cancelAnimationFrame(S.id) : clearTimeout(S.id), S.id = null) }

                function u(e, t, o, a, n) {
                    switch (n) {
                        case "linear":
                        case "mcsLinear":
                            return o * e / a + t;
                        case "mcsLinearOut":
                            return e /= a, e--, o * Math.sqrt(1 - e * e) + t;
                        case "easeInOutSmooth":
                            return e /= a / 2, 1 > e ? o / 2 * e * e + t : (e--, -o / 2 * (e * (e - 2) - 1) + t);
                        case "easeInOutStrong":
                            return e /= a / 2, 1 > e ? o / 2 * Math.pow(2, 10 * (e - 1)) + t : (e--, o / 2 * (-Math.pow(2, -10 * e) + 2) + t);
                        case "easeInOut":
                        case "mcsEaseInOut":
                            return e /= a / 2, 1 > e ? o / 2 * e * e * e + t : (e -= 2, o / 2 * (e * e * e + 2) + t);
                        case "easeOutSmooth":
                            return e /= a, e--, -o * (e * e * e * e - 1) + t;
                        case "easeOutStrong":
                            return o * (-Math.pow(2, -10 * e / a) + 1) + t;
                        case "easeOut":
                        case "mcsEaseOut":
                        default:
                            var i = (e /= a) * e,
                                r = i * e;
                            return t + o * (.499999999999997 * r * i + -2.5 * i * i + 5.5 * r + -6.5 * i + 4 * e)
                    }
                }
                e._mTween || (e._mTween = { top: {}, left: {} });
                var f, h, r = r || {},
                    m = r.onStart || function() {},
                    p = r.onUpdate || function() {},
                    g = r.onComplete || function() {},
                    v = K(),
                    x = 0,
                    _ = e.offsetTop,
                    w = e.style,
                    S = e._mTween[t];
                "left" === t && (_ = e.offsetLeft);
                var b = o - _;
                S.stop = 0, "none" !== i && d(), c()
            },
            K = function() { return window.performance && window.performance.now ? window.performance.now() : window.performance && window.performance.webkitNow ? window.performance.webkitNow() : Date.now ? Date.now() : (new Date).getTime() },
            Z = function() {
                var e = this;
                e._mTween || (e._mTween = { top: {}, left: {} });
                for (var t = ["top", "left"], o = 0; o < t.length; o++) {
                    var a = t[o];
                    e._mTween[a].id && (window.requestAnimationFrame ? window.cancelAnimationFrame(e._mTween[a].id) : clearTimeout(e._mTween[a].id), e._mTween[a].id = null, e._mTween[a].stop = 1)
                }
            },
            $ = function(e, t) { try { delete e[t] } catch (o) { e[t] = null } },
            ee = function(e) { return !(e.which && 1 !== e.which) },
            te = function(e) { var t = e.originalEvent.pointerType; return !(t && "touch" !== t && 2 !== t) },
            oe = function(e) { return !isNaN(parseFloat(e)) && isFinite(e) },
            ae = function(e) { var t = e.parents(".mCSB_container"); return [e.offset().top - t.offset().top, e.offset().left - t.offset().left] },
            ne = function() {
                function e() {
                    var e = ["webkit", "moz", "ms", "o"];
                    if ("hidden" in document) return "hidden";
                    for (var t = 0; t < e.length; t++)
                        if (e[t] + "Hidden" in document) return e[t] + "Hidden";
                    return null
                }
                var t = e();
                return t ? document[t] : !1
            };
        e.fn[o] = function(t) { return u[t] ? u[t].apply(this, Array.prototype.slice.call(arguments, 1)) : "object" != typeof t && t ? void e.error("Method " + t + " does not exist") : u.init.apply(this, arguments) }, e[o] = function(t) { return u[t] ? u[t].apply(this, Array.prototype.slice.call(arguments, 1)) : "object" != typeof t && t ? void e.error("Method " + t + " does not exist") : u.init.apply(this, arguments) }, e[o].defaults = i, window[o] = !0, e(window).bind("load", function() {
            e(n)[o](), e.extend(e.expr[":"], {
                mcsInView: e.expr[":"].mcsInView || function(t) {
                    var o, a, n = e(t),
                        i = n.parents(".mCSB_container");
                    if (i.length) return o = i.parent(), a = [i[0].offsetTop, i[0].offsetLeft], a[0] + ae(n)[0] >= 0 && a[0] + ae(n)[0] < o.height() - n.outerHeight(!1) && a[1] + ae(n)[1] >= 0 && a[1] + ae(n)[1] < o.width() - n.outerWidth(!1)
                },
                mcsInSight: e.expr[":"].mcsInSight || function(t, o, a) {
                    var n, i, r, l, s = e(t),
                        c = s.parents(".mCSB_container"),
                        d = "exact" === a[3] ? [
                            [1, 0],
                            [1, 0]
                        ] : [
                            [.9, .1],
                            [.6, .4]
                        ];
                    if (c.length) return n = [s.outerHeight(!1), s.outerWidth(!1)], r = [c[0].offsetTop + ae(s)[0], c[0].offsetLeft + ae(s)[1]], i = [c.parent()[0].offsetHeight, c.parent()[0].offsetWidth], l = [n[0] < i[0] ? d[0] : d[1], n[1] < i[1] ? d[0] : d[1]], r[0] - i[0] * l[0][0] < 0 && r[0] + n[0] - i[0] * l[0][1] >= 0 && r[1] - i[1] * l[1][0] < 0 && r[1] + n[1] - i[1] * l[1][1] >= 0
                },
                mcsOverflow: e.expr[":"].mcsOverflow || function(t) { var o = e(t).data(a); if (o) return o.overflowed[0] || o.overflowed[1] }
            })
        })
    })
});

/* UItoTop jQuery Plugin 1.2 | Matt Varone | https://www.mattvarone.com/web-design/uitotop-jquery-plugin */

(function($) {
    $.fn.UItoTop = function(options) {
        var defaults = { text: 'To Top', min: 200, inDelay: 600, outDelay: 400, containerID: 'toTop', containerHoverID: 'toTopHover', scrollSpeed: 1200, easingType: 'linear' },
            settings = $.extend(defaults, options),
            containerIDhash = '#' + settings.containerID,
            containerHoverIDHash = '#' + settings.containerHoverID;
        $('body').append('<a href="#" id="' + settings.containerID + '">' + settings.text + '</a>');
        $(containerIDhash).hide().on('click.UItoTop', function() {
            $('html, body').animate({ scrollTop: 0 }, settings.scrollSpeed, settings.easingType);
            $('#' + settings.containerHoverID, this).stop().animate({ 'opacity': 0 }, settings.inDelay, settings.easingType);
            return false;
        }).prepend('<span id="' + settings.containerHoverID + '"></span>').hover(function() { $(containerHoverIDHash, this).stop().animate({ 'opacity': 1 }, 600, 'linear'); }, function() { $(containerHoverIDHash, this).stop().animate({ 'opacity': 0 }, 700, 'linear'); });
        $(window).scroll(function() {
            var sd = $(window).scrollTop();
            if (typeof document.body.style.maxHeight === "undefined") { $(containerIDhash).css({ 'position': 'absolute', 'top': sd + $(window).height() - 50 }); }
            if (sd > settings.min)
                $(containerIDhash).fadeIn(settings.inDelay);
            else
                $(containerIDhash).fadeOut(settings.Outdelay);
        });
    };
})(jQuery);


/*
 *  jQuery OwlCarousel v1.3.3
 *
 *  Copyright (c) 2013 Bartosz Wojciechowski
 *  https://www.owlgraphic.com/owlcarousel/
 *
 *  Licensed under MIT
 *
 */

"function" !== typeof Object.create && (Object.create = function(f) {
    function g() {}
    g.prototype = f;
    return new g
});
(function(f, g, k) {
    var l = {
        init: function(a, b) {
            this.$elem = f(b);
            this.options = f.extend({}, f.fn.owlCarousel.options, this.$elem.data(), a);
            this.userOptions = a;
            this.loadContent()
        },
        loadContent: function() {
            function a(a) {
                var d, e = "";
                if ("function" === typeof b.options.jsonSuccess) b.options.jsonSuccess.apply(this, [a]);
                else {
                    for (d in a.owl) a.owl.hasOwnProperty(d) && (e += a.owl[d].item);
                    b.$elem.html(e)
                }
                b.logIn()
            }
            var b = this,
                e;
            "function" === typeof b.options.beforeInit && b.options.beforeInit.apply(this, [b.$elem]);
            "string" === typeof b.options.jsonPath ?
                (e = b.options.jsonPath, f.getJSON(e, a)) : b.logIn()
        },
        logIn: function() {
            this.$elem.data("owl-originalStyles", this.$elem.attr("style"));
            this.$elem.data("owl-originalClasses", this.$elem.attr("class"));
            this.$elem.css({ opacity: 0 });
            this.orignalItems = this.options.items;
            this.checkBrowser();
            this.wrapperWidth = 0;
            this.checkVisible = null;
            this.setVars()
        },
        setVars: function() {
            if (0 === this.$elem.children().length) return !1;
            this.baseClass();
            this.eventTypes();
            this.$userItems = this.$elem.children();
            this.itemsAmount = this.$userItems.length;
            this.wrapItems();
            this.$owlItems = this.$elem.find(".owl-item");
            this.$owlWrapper = this.$elem.find(".owl-wrapper");
            this.playDirection = "next";
            this.prevItem = 0;
            this.prevArr = [0];
            this.currentItem = 0;
            this.customEvents();
            this.onStartup()
        },
        onStartup: function() {
            this.updateItems();
            this.calculateAll();
            this.buildControls();
            this.updateControls();
            this.response();
            this.moveEvents();
            this.stopOnHover();
            this.owlStatus();
            !1 !== this.options.transitionStyle && this.transitionTypes(this.options.transitionStyle);
            !0 === this.options.autoPlay &&
                (this.options.autoPlay = 5E3);
            this.play();
            this.$elem.find(".owl-wrapper").css("display", "block");
            this.$elem.is(":visible") ? this.$elem.css("opacity", 1) : this.watchVisibility();
            this.onstartup = !1;
            this.eachMoveUpdate();
            "function" === typeof this.options.afterInit && this.options.afterInit.apply(this, [this.$elem])
        },
        eachMoveUpdate: function() {
            !0 === this.options.lazyLoad && this.lazyLoad();
            !0 === this.options.autoHeight && this.autoHeight();
            this.onVisibleItems();
            "function" === typeof this.options.afterAction && this.options.afterAction.apply(this, [this.$elem])
        },
        updateVars: function() {
            "function" === typeof this.options.beforeUpdate && this.options.beforeUpdate.apply(this, [this.$elem]);
            this.watchVisibility();
            this.updateItems();
            this.calculateAll();
            this.updatePosition();
            this.updateControls();
            this.eachMoveUpdate();
            "function" === typeof this.options.afterUpdate && this.options.afterUpdate.apply(this, [this.$elem])
        },
        reload: function() {
            var a = this;
            g.setTimeout(function() { a.updateVars() }, 0)
        },
        watchVisibility: function() {
            var a = this;
            if (!1 === a.$elem.is(":visible")) a.$elem.css({ opacity: 0 }),
                g.clearInterval(a.autoPlayInterval), g.clearInterval(a.checkVisible);
            else return !1;
            a.checkVisible = g.setInterval(function() { a.$elem.is(":visible") && (a.reload(), a.$elem.animate({ opacity: 1 }, 200), g.clearInterval(a.checkVisible)) }, 500)
        },
        wrapItems: function() {
            this.$userItems.wrapAll('<div class="owl-wrapper">').wrap('<div class="owl-item"></div>');
            this.$elem.find(".owl-wrapper").wrap('<div class="owl-wrapper-outer">');
            this.wrapperOuter = this.$elem.find(".owl-wrapper-outer");
            this.$elem.css("display", "block")
        },
        baseClass: function() {
            var a = this.$elem.hasClass(this.options.baseClass),
                b = this.$elem.hasClass(this.options.theme);
            a || this.$elem.addClass(this.options.baseClass);
            b || this.$elem.addClass(this.options.theme)
        },
        updateItems: function() {
            var a, b;
            if (!1 === this.options.responsive) return !1;
            if (!0 === this.options.singleItem) return this.options.items = this.orignalItems = 1, this.options.itemsCustom = !1, this.options.itemsDesktop = !1, this.options.itemsDesktopSmall = !1, this.options.itemsTablet = !1, this.options.itemsTabletSmall = !1, this.options.itemsMobile = !1;
            a = f(this.options.responsiveBaseWidth).width();
            a > (this.options.itemsDesktop[0] || this.orignalItems) && (this.options.items = this.orignalItems);
            if (!1 !== this.options.itemsCustom)
                for (this.options.itemsCustom.sort(function(a, b) { return a[0] - b[0] }), b = 0; b < this.options.itemsCustom.length; b += 1) this.options.itemsCustom[b][0] <= a && (this.options.items = this.options.itemsCustom[b][1]);
            else a <= this.options.itemsDesktop[0] && !1 !== this.options.itemsDesktop && (this.options.items = this.options.itemsDesktop[1]),
                a <= this.options.itemsDesktopSmall[0] && !1 !== this.options.itemsDesktopSmall && (this.options.items = this.options.itemsDesktopSmall[1]), a <= this.options.itemsTablet[0] && !1 !== this.options.itemsTablet && (this.options.items = this.options.itemsTablet[1]), a <= this.options.itemsTabletSmall[0] && !1 !== this.options.itemsTabletSmall && (this.options.items = this.options.itemsTabletSmall[1]), a <= this.options.itemsMobile[0] && !1 !== this.options.itemsMobile && (this.options.items = this.options.itemsMobile[1]);
            this.options.items > this.itemsAmount &&
                !0 === this.options.itemsScaleUp && (this.options.items = this.itemsAmount)
        },
        response: function() {
            var a = this,
                b, e;
            if (!0 !== a.options.responsive) return !1;
            e = f(g).width();
            a.resizer = function() {
                f(g).width() !== e && (!1 !== a.options.autoPlay && g.clearInterval(a.autoPlayInterval), g.clearTimeout(b), b = g.setTimeout(function() {
                    e = f(g).width();
                    a.updateVars()
                }, a.options.responsiveRefreshRate))
            };
            f(g).resize(a.resizer)
        },
        updatePosition: function() { this.jumpTo(this.currentItem);!1 !== this.options.autoPlay && this.checkAp() },
        appendItemsSizes: function() {
            var a =
                this,
                b = 0,
                e = a.itemsAmount - a.options.items;
            a.$owlItems.each(function(c) {
                var d = f(this);
                d.css({ width: a.itemWidth }).data("owl-item", Number(c));
                if (0 === c % a.options.items || c === e) c > e || (b += 1);
                d.data("owl-roundPages", b)
            })
        },
        appendWrapperSizes: function() {
            this.$owlWrapper.css({ width: this.$owlItems.length * this.itemWidth * 2, left: 0 });
            this.appendItemsSizes()
        },
        calculateAll: function() {
            this.calculateWidth();
            this.appendWrapperSizes();
            this.loops();
            this.max()
        },
        calculateWidth: function() {
            this.itemWidth = Math.round(this.$elem.width() /
                this.options.items)
        },
        max: function() {
            var a = -1 * (this.itemsAmount * this.itemWidth - this.options.items * this.itemWidth);
            this.options.items > this.itemsAmount ? this.maximumPixels = a = this.maximumItem = 0 : (this.maximumItem = this.itemsAmount - this.options.items, this.maximumPixels = a);
            return a
        },
        min: function() { return 0 },
        loops: function() {
            var a = 0,
                b = 0,
                e, c;
            this.positionsInArray = [0];
            this.pagesInArray = [];
            for (e = 0; e < this.itemsAmount; e += 1) b += this.itemWidth, this.positionsInArray.push(-b), !0 === this.options.scrollPerPage && (c = f(this.$owlItems[e]),
                c = c.data("owl-roundPages"), c !== a && (this.pagesInArray[a] = this.positionsInArray[e], a = c))
        },
        buildControls: function() { if (!0 === this.options.navigation || !0 === this.options.pagination) this.owlControls = f('<div class="owl-controls"/>').toggleClass("clickable", !this.browser.isTouch).appendTo(this.$elem);!0 === this.options.pagination && this.buildPagination();!0 === this.options.navigation && this.buildButtons() },
        buildButtons: function() {
            var a = this,
                b = f('<div class="owl-buttons"/>');
            a.owlControls.append(b);
            a.buttonPrev =
                f("<div/>", { "class": "owl-prev", html: a.options.navigationText[0] || "" });
            a.buttonNext = f("<div/>", { "class": "owl-next", html: a.options.navigationText[1] || "" });
            b.append(a.buttonPrev).append(a.buttonNext);
            b.on("touchstart.owlControls mousedown.owlControls", 'div[class^="owl"]', function(a) { a.preventDefault() });
            b.on("touchend.owlControls mouseup.owlControls", 'div[class^="owl"]', function(b) {
                b.preventDefault();
                f(this).hasClass("owl-next") ? a.next() : a.prev()
            })
        },
        buildPagination: function() {
            var a = this;
            a.paginationWrapper =
                f('<div class="owl-pagination"/>');
            a.owlControls.append(a.paginationWrapper);
            a.paginationWrapper.on("touchend.owlControls mouseup.owlControls", ".owl-page", function(b) {
                b.preventDefault();
                Number(f(this).data("owl-page")) !== a.currentItem && a.goTo(Number(f(this).data("owl-page")), !0)
            })
        },
        updatePagination: function() {
            var a, b, e, c, d, g;
            if (!1 === this.options.pagination) return !1;
            this.paginationWrapper.html("");
            a = 0;
            b = this.itemsAmount - this.itemsAmount % this.options.items;
            for (c = 0; c < this.itemsAmount; c += 1) 0 === c % this.options.items &&
                (a += 1, b === c && (e = this.itemsAmount - this.options.items), d = f("<div/>", { "class": "owl-page" }), g = f("<span></span>", { text: !0 === this.options.paginationNumbers ? a : "", "class": !0 === this.options.paginationNumbers ? "owl-numbers" : "" }), d.append(g), d.data("owl-page", b === c ? e : c), d.data("owl-roundPages", a), this.paginationWrapper.append(d));
            this.checkPagination()
        },
        checkPagination: function() {
            var a = this;
            if (!1 === a.options.pagination) return !1;
            a.paginationWrapper.find(".owl-page").each(function() {
                f(this).data("owl-roundPages") ===
                    f(a.$owlItems[a.currentItem]).data("owl-roundPages") && (a.paginationWrapper.find(".owl-page").removeClass("active"), f(this).addClass("active"))
            })
        },
        checkNavigation: function() {
            if (!1 === this.options.navigation) return !1;
            !1 === this.options.rewindNav && (0 === this.currentItem && 0 === this.maximumItem ? (this.buttonPrev.addClass("disabled"), this.buttonNext.addClass("disabled")) : 0 === this.currentItem && 0 !== this.maximumItem ? (this.buttonPrev.addClass("disabled"), this.buttonNext.removeClass("disabled")) : this.currentItem ===
                this.maximumItem ? (this.buttonPrev.removeClass("disabled"), this.buttonNext.addClass("disabled")) : 0 !== this.currentItem && this.currentItem !== this.maximumItem && (this.buttonPrev.removeClass("disabled"), this.buttonNext.removeClass("disabled")))
        },
        updateControls: function() {
            this.updatePagination();
            this.checkNavigation();
            this.owlControls && (this.options.items >= this.itemsAmount ? this.owlControls.hide() : this.owlControls.show())
        },
        destroyControls: function() { this.owlControls && this.owlControls.remove() },
        next: function(a) {
            if (this.isTransition) return !1;
            this.currentItem += !0 === this.options.scrollPerPage ? this.options.items : 1;
            if (this.currentItem > this.maximumItem + (!0 === this.options.scrollPerPage ? this.options.items - 1 : 0))
                if (!0 === this.options.rewindNav) this.currentItem = 0, a = "rewind";
                else return this.currentItem = this.maximumItem, !1;
            this.goTo(this.currentItem, a)
        },
        prev: function(a) {
            if (this.isTransition) return !1;
            this.currentItem = !0 === this.options.scrollPerPage && 0 < this.currentItem && this.currentItem < this.options.items ? 0 : this.currentItem - (!0 === this.options.scrollPerPage ?
                this.options.items : 1);
            if (0 > this.currentItem)
                if (!0 === this.options.rewindNav) this.currentItem = this.maximumItem, a = "rewind";
                else return this.currentItem = 0, !1;
            this.goTo(this.currentItem, a)
        },
        goTo: function(a, b, e) {
            var c = this;
            if (c.isTransition) return !1;
            "function" === typeof c.options.beforeMove && c.options.beforeMove.apply(this, [c.$elem]);
            a >= c.maximumItem ? a = c.maximumItem : 0 >= a && (a = 0);
            c.currentItem = c.owl.currentItem = a;
            if (!1 !== c.options.transitionStyle && "drag" !== e && 1 === c.options.items && !0 === c.browser.support3d) return c.swapSpeed(0), !0 === c.browser.support3d ? c.transition3d(c.positionsInArray[a]) : c.css2slide(c.positionsInArray[a], 1), c.afterGo(), c.singleItemTransition(), !1;
            a = c.positionsInArray[a];
            !0 === c.browser.support3d ? (c.isCss3Finish = !1, !0 === b ? (c.swapSpeed("paginationSpeed"), g.setTimeout(function() { c.isCss3Finish = !0 }, c.options.paginationSpeed)) : "rewind" === b ? (c.swapSpeed(c.options.rewindSpeed), g.setTimeout(function() { c.isCss3Finish = !0 }, c.options.rewindSpeed)) : (c.swapSpeed("slideSpeed"), g.setTimeout(function() { c.isCss3Finish = !0 },
                c.options.slideSpeed)), c.transition3d(a)) : !0 === b ? c.css2slide(a, c.options.paginationSpeed) : "rewind" === b ? c.css2slide(a, c.options.rewindSpeed) : c.css2slide(a, c.options.slideSpeed);
            c.afterGo()
        },
        jumpTo: function(a) {
            "function" === typeof this.options.beforeMove && this.options.beforeMove.apply(this, [this.$elem]);
            a >= this.maximumItem || -1 === a ? a = this.maximumItem : 0 >= a && (a = 0);
            this.swapSpeed(0);
            !0 === this.browser.support3d ? this.transition3d(this.positionsInArray[a]) : this.css2slide(this.positionsInArray[a], 1);
            this.currentItem =
                this.owl.currentItem = a;
            this.afterGo()
        },
        afterGo: function() {
            this.prevArr.push(this.currentItem);
            this.prevItem = this.owl.prevItem = this.prevArr[this.prevArr.length - 2];
            this.prevArr.shift(0);
            this.prevItem !== this.currentItem && (this.checkPagination(), this.checkNavigation(), this.eachMoveUpdate(), !1 !== this.options.autoPlay && this.checkAp());
            "function" === typeof this.options.afterMove && this.prevItem !== this.currentItem && this.options.afterMove.apply(this, [this.$elem])
        },
        stop: function() {
            this.apStatus = "stop";
            g.clearInterval(this.autoPlayInterval)
        },
        checkAp: function() { "stop" !== this.apStatus && this.play() },
        play: function() {
            var a = this;
            a.apStatus = "play";
            if (!1 === a.options.autoPlay) return !1;
            g.clearInterval(a.autoPlayInterval);
            a.autoPlayInterval = g.setInterval(function() { a.next(!0) }, a.options.autoPlay)
        },
        swapSpeed: function(a) { "slideSpeed" === a ? this.$owlWrapper.css(this.addCssSpeed(this.options.slideSpeed)) : "paginationSpeed" === a ? this.$owlWrapper.css(this.addCssSpeed(this.options.paginationSpeed)) : "string" !== typeof a && this.$owlWrapper.css(this.addCssSpeed(a)) },
        addCssSpeed: function(a) { return { "-webkit-transition": "all " + a + "ms ease", "-moz-transition": "all " + a + "ms ease", "-o-transition": "all " + a + "ms ease", transition: "all " + a + "ms ease" } },
        removeTransition: function() { return { "-webkit-transition": "", "-moz-transition": "", "-o-transition": "", transition: "" } },
        doTranslate: function(a) {
            return {
                "-webkit-transform": "translate3d(" + a + "px, 0px, 0px)",
                "-moz-transform": "translate3d(" + a + "px, 0px, 0px)",
                "-o-transform": "translate3d(" + a + "px, 0px, 0px)",
                "-ms-transform": "translate3d(" +
                    a + "px, 0px, 0px)",
                transform: "translate3d(" + a + "px, 0px,0px)"
            }
        },
        transition3d: function(a) { this.$owlWrapper.css(this.doTranslate(a)) },
        css2move: function(a) { this.$owlWrapper.css({ left: a }) },
        css2slide: function(a, b) {
            var e = this;
            e.isCssFinish = !1;
            e.$owlWrapper.stop(!0, !0).animate({ left: a }, { duration: b || e.options.slideSpeed, complete: function() { e.isCssFinish = !0 } })
        },
        checkBrowser: function() {
            var a = k.createElement("div");
            a.style.cssText = "  -moz-transform:translate3d(0px, 0px, 0px); -ms-transform:translate3d(0px, 0px, 0px); -o-transform:translate3d(0px, 0px, 0px); -webkit-transform:translate3d(0px, 0px, 0px); transform:translate3d(0px, 0px, 0px)";
            a = a.style.cssText.match(/translate3d\(0px, 0px, 0px\)/g);
            this.browser = { support3d: null !== a && 1 === a.length, isTouch: "ontouchstart" in g || g.navigator.msMaxTouchPoints }
        },
        moveEvents: function() { if (!1 !== this.options.mouseDrag || !1 !== this.options.touchDrag) this.gestures(), this.disabledEvents() },
        eventTypes: function() {
            var a = ["s", "e", "x"];
            this.ev_types = {};
            !0 === this.options.mouseDrag && !0 === this.options.touchDrag ? a = ["touchstart.owl mousedown.owl", "touchmove.owl mousemove.owl", "touchend.owl touchcancel.owl mouseup.owl"] :
                !1 === this.options.mouseDrag && !0 === this.options.touchDrag ? a = ["touchstart.owl", "touchmove.owl", "touchend.owl touchcancel.owl"] : !0 === this.options.mouseDrag && !1 === this.options.touchDrag && (a = ["mousedown.owl", "mousemove.owl", "mouseup.owl"]);
            this.ev_types.start = a[0];
            this.ev_types.move = a[1];
            this.ev_types.end = a[2]
        },
        disabledEvents: function() {
            this.$elem.on("dragstart.owl", function(a) { a.preventDefault() });
            this.$elem.on("mousedown.disableTextSelect", function(a) { return f(a.target).is("input, textarea, select, option") })
        },
        gestures: function() {
            function a(a) { if (void 0 !== a.touches) return { x: a.touches[0].pageX, y: a.touches[0].pageY }; if (void 0 === a.touches) { if (void 0 !== a.pageX) return { x: a.pageX, y: a.pageY }; if (void 0 === a.pageX) return { x: a.clientX, y: a.clientY } } }

            function b(a) { "on" === a ? (f(k).on(d.ev_types.move, e), f(k).on(d.ev_types.end, c)) : "off" === a && (f(k).off(d.ev_types.move), f(k).off(d.ev_types.end)) }

            function e(b) {
                b = b.originalEvent || b || g.event;
                d.newPosX = a(b).x - h.offsetX;
                d.newPosY = a(b).y - h.offsetY;
                d.newRelativeX = d.newPosX - h.relativePos;
                "function" === typeof d.options.startDragging && !0 !== h.dragging && 0 !== d.newRelativeX && (h.dragging = !0, d.options.startDragging.apply(d, [d.$elem]));
                (8 < d.newRelativeX || -8 > d.newRelativeX) && !0 === d.browser.isTouch && (void 0 !== b.preventDefault ? b.preventDefault() : b.returnValue = !1, h.sliding = !0);
                (10 < d.newPosY || -10 > d.newPosY) && !1 === h.sliding && f(k).off("touchmove.owl");
                d.newPosX = Math.max(Math.min(d.newPosX, d.newRelativeX / 5), d.maximumPixels + d.newRelativeX / 5);
                !0 === d.browser.support3d ? d.transition3d(d.newPosX) : d.css2move(d.newPosX)
            }

            function c(a) {
                a = a.originalEvent || a || g.event;
                var c;
                a.target = a.target || a.srcElement;
                h.dragging = !1;
                !0 !== d.browser.isTouch && d.$owlWrapper.removeClass("grabbing");
                d.dragDirection = 0 > d.newRelativeX ? d.owl.dragDirection = "left" : d.owl.dragDirection = "right";
                0 !== d.newRelativeX && (c = d.getNewPosition(), d.goTo(c, !1, "drag"), h.targetElement === a.target && !0 !== d.browser.isTouch && (f(a.target).on("click.disable", function(a) {
                        a.stopImmediatePropagation();
                        a.stopPropagation();
                        a.preventDefault();
                        f(a.target).off("click.disable")
                    }),
                    a = f._data(a.target, "events").click, c = a.pop(), a.splice(0, 0, c)));
                b("off")
            }
            var d = this,
                h = { offsetX: 0, offsetY: 0, baseElWidth: 0, relativePos: 0, position: null, minSwipe: null, maxSwipe: null, sliding: null, dargging: null, targetElement: null };
            d.isCssFinish = !0;
            d.$elem.on(d.ev_types.start, ".owl-wrapper", function(c) {
                c = c.originalEvent || c || g.event;
                var e;
                if (3 === c.which) return !1;
                if (!(d.itemsAmount <= d.options.items)) {
                    if (!1 === d.isCssFinish && !d.options.dragBeforeAnimFinish || !1 === d.isCss3Finish && !d.options.dragBeforeAnimFinish) return !1;
                    !1 !== d.options.autoPlay && g.clearInterval(d.autoPlayInterval);
                    !0 === d.browser.isTouch || d.$owlWrapper.hasClass("grabbing") || d.$owlWrapper.addClass("grabbing");
                    d.newPosX = 0;
                    d.newRelativeX = 0;
                    f(this).css(d.removeTransition());
                    e = f(this).position();
                    h.relativePos = e.left;
                    h.offsetX = a(c).x - e.left;
                    h.offsetY = a(c).y - e.top;
                    b("on");
                    h.sliding = !1;
                    h.targetElement = c.target || c.srcElement
                }
            })
        },
        getNewPosition: function() {
            var a = this.closestItem();
            a > this.maximumItem ? a = this.currentItem = this.maximumItem : 0 <= this.newPosX && (this.currentItem =
                a = 0);
            return a
        },
        closestItem: function() {
            var a = this,
                b = !0 === a.options.scrollPerPage ? a.pagesInArray : a.positionsInArray,
                e = a.newPosX,
                c = null;
            f.each(b, function(d, g) {
                e - a.itemWidth / 20 > b[d + 1] && e - a.itemWidth / 20 < g && "left" === a.moveDirection() ? (c = g, a.currentItem = !0 === a.options.scrollPerPage ? f.inArray(c, a.positionsInArray) : d) : e + a.itemWidth / 20 < g && e + a.itemWidth / 20 > (b[d + 1] || b[d] - a.itemWidth) && "right" === a.moveDirection() && (!0 === a.options.scrollPerPage ? (c = b[d + 1] || b[b.length - 1], a.currentItem = f.inArray(c, a.positionsInArray)) :
                    (c = b[d + 1], a.currentItem = d + 1))
            });
            return a.currentItem
        },
        moveDirection: function() {
            var a;
            0 > this.newRelativeX ? (a = "right", this.playDirection = "next") : (a = "left", this.playDirection = "prev");
            return a
        },
        customEvents: function() {
            var a = this;
            a.$elem.on("owl.next", function() { a.next() });
            a.$elem.on("owl.prev", function() { a.prev() });
            a.$elem.on("owl.play", function(b, e) {
                a.options.autoPlay = e;
                a.play();
                a.hoverStatus = "play"
            });
            a.$elem.on("owl.stop", function() {
                a.stop();
                a.hoverStatus = "stop"
            });
            a.$elem.on("owl.goTo", function(b, e) { a.goTo(e) });
            a.$elem.on("owl.jumpTo", function(b, e) { a.jumpTo(e) })
        },
        stopOnHover: function() { var a = this;!0 === a.options.stopOnHover && !0 !== a.browser.isTouch && !1 !== a.options.autoPlay && (a.$elem.on("mouseover", function() { a.stop() }), a.$elem.on("mouseout", function() { "stop" !== a.hoverStatus && a.play() })) },
        lazyLoad: function() {
            var a, b, e, c, d;
            if (!1 === this.options.lazyLoad) return !1;
            for (a = 0; a < this.itemsAmount; a += 1) b = f(this.$owlItems[a]), "loaded" !== b.data("owl-loaded") && (e = b.data("owl-item"), c = b.find(".lazyOwl"), "string" !== typeof c.data("src") ?
                b.data("owl-loaded", "loaded") : (void 0 === b.data("owl-loaded") && (c.hide(), b.addClass("loading").data("owl-loaded", "checked")), (d = !0 === this.options.lazyFollow ? e >= this.currentItem : !0) && e < this.currentItem + this.options.items && c.length && this.lazyPreload(b, c)))
        },
        lazyPreload: function(a, b) {
            function e() {
                a.data("owl-loaded", "loaded").removeClass("loading");
                b.removeAttr("data-src");
                "fade" === d.options.lazyEffect ? b.fadeIn(400) : b.show();
                "function" === typeof d.options.afterLazyLoad && d.options.afterLazyLoad.apply(this, [d.$elem])
            }

            function c() {
                f += 1;
                d.completeImg(b.get(0)) || !0 === k ? e() : 100 >= f ? g.setTimeout(c, 100) : e()
            }
            var d = this,
                f = 0,
                k;
            "DIV" === b.prop("tagName") ? (b.css("background-image", "url(" + b.data("src") + ")"), k = !0) : b[0].src = b.data("src");
            c()
        },
        autoHeight: function() {
            function a() {
                var a = f(e.$owlItems[e.currentItem]).height();
                e.wrapperOuter.css("height", a + "px");
                e.wrapperOuter.hasClass("autoHeight") || g.setTimeout(function() { e.wrapperOuter.addClass("autoHeight") }, 0)
            }

            function b() {
                d += 1;
                e.completeImg(c.get(0)) ? a() : 100 >= d ? g.setTimeout(b,
                    100) : e.wrapperOuter.css("height", "")
            }
            var e = this,
                c = f(e.$owlItems[e.currentItem]).find("img"),
                d;
            void 0 !== c.get(0) ? (d = 0, b()) : a()
        },
        completeImg: function(a) { return !a.complete || "undefined" !== typeof a.naturalWidth && 0 === a.naturalWidth ? !1 : !0 },
        onVisibleItems: function() {
            var a;
            !0 === this.options.addClassActive && this.$owlItems.removeClass("active");
            this.visibleItems = [];
            for (a = this.currentItem; a < this.currentItem + this.options.items; a += 1) this.visibleItems.push(a), !0 === this.options.addClassActive && f(this.$owlItems[a]).addClass("active");
            this.owl.visibleItems = this.visibleItems
        },
        transitionTypes: function(a) {
            this.outClass = "owl-" + a + "-out";
            this.inClass = "owl-" + a + "-in"
        },
        singleItemTransition: function() {
            var a = this,
                b = a.outClass,
                e = a.inClass,
                c = a.$owlItems.eq(a.currentItem),
                d = a.$owlItems.eq(a.prevItem),
                f = Math.abs(a.positionsInArray[a.currentItem]) + a.positionsInArray[a.prevItem],
                g = Math.abs(a.positionsInArray[a.currentItem]) + a.itemWidth / 2;
            a.isTransition = !0;
            a.$owlWrapper.addClass("owl-origin").css({
                "-webkit-transform-origin": g + "px",
                "-moz-perspective-origin": g +
                    "px",
                "perspective-origin": g + "px"
            });
            d.css({ position: "relative", left: f + "px" }).addClass(b).on("webkitAnimationEnd oAnimationEnd MSAnimationEnd animationend", function() {
                a.endPrev = !0;
                d.off("webkitAnimationEnd oAnimationEnd MSAnimationEnd animationend");
                a.clearTransStyle(d, b)
            });
            c.addClass(e).on("webkitAnimationEnd oAnimationEnd MSAnimationEnd animationend", function() {
                a.endCurrent = !0;
                c.off("webkitAnimationEnd oAnimationEnd MSAnimationEnd animationend");
                a.clearTransStyle(c, e)
            })
        },
        clearTransStyle: function(a,
            b) {
            a.css({ position: "", left: "" }).removeClass(b);
            this.endPrev && this.endCurrent && (this.$owlWrapper.removeClass("owl-origin"), this.isTransition = this.endCurrent = this.endPrev = !1)
        },
        owlStatus: function() { this.owl = { userOptions: this.userOptions, baseElement: this.$elem, userItems: this.$userItems, owlItems: this.$owlItems, currentItem: this.currentItem, prevItem: this.prevItem, visibleItems: this.visibleItems, isTouch: this.browser.isTouch, browser: this.browser, dragDirection: this.dragDirection } },
        clearEvents: function() {
            this.$elem.off(".owl owl mousedown.disableTextSelect");
            f(k).off(".owl owl");
            f(g).off("resize", this.resizer)
        },
        unWrap: function() {
            0 !== this.$elem.children().length && (this.$owlWrapper.unwrap(), this.$userItems.unwrap().unwrap(), this.owlControls && this.owlControls.remove());
            this.clearEvents();
            this.$elem.attr("style", this.$elem.data("owl-originalStyles") || "").attr("class", this.$elem.data("owl-originalClasses"))
        },
        destroy: function() {
            this.stop();
            g.clearInterval(this.checkVisible);
            this.unWrap();
            this.$elem.removeData()
        },
        reinit: function(a) {
            a = f.extend({}, this.userOptions,
                a);
            this.unWrap();
            this.init(a, this.$elem)
        },
        addItem: function(a, b) {
            var e;
            if (!a) return !1;
            if (0 === this.$elem.children().length) return this.$elem.append(a), this.setVars(), !1;
            this.unWrap();
            e = void 0 === b || -1 === b ? -1 : b;
            e >= this.$userItems.length || -1 === e ? this.$userItems.eq(-1).after(a) : this.$userItems.eq(e).before(a);
            this.setVars()
        },
        removeItem: function(a) {
            if (0 === this.$elem.children().length) return !1;
            a = void 0 === a || -1 === a ? -1 : a;
            this.unWrap();
            this.$userItems.eq(a).remove();
            this.setVars()
        }
    };
    f.fn.owlCarousel = function(a) {
        return this.each(function() {
            if (!0 ===
                f(this).data("owl-init")) return !1;
            f(this).data("owl-init", !0);
            var b = Object.create(l);
            b.init(a, this);
            f.data(this, "owlCarousel", b)
        })
    };
    f.fn.owlCarousel.options = {
        items: 5,
        itemsCustom: !1,
        itemsDesktop: [1199, 4],
        itemsDesktopSmall: [979, 3],
        itemsTablet: [768, 2],
        itemsTabletSmall: !1,
        itemsMobile: [479, 1],
        singleItem: !1,
        itemsScaleUp: !1,
        slideSpeed: 200,
        paginationSpeed: 800,
        rewindSpeed: 1E3,
        autoPlay: !1,
        stopOnHover: !1,
        navigation: !1,
        navigationText: ["prev", "next"],
        rewindNav: !0,
        scrollPerPage: !1,
        pagination: !0,
        paginationNumbers: !1,
        responsive: !0,
        responsiveRefreshRate: 200,
        responsiveBaseWidth: g,
        baseClass: "owl-carousel",
        theme: "owl-theme",
        lazyLoad: !1,
        lazyFollow: !0,
        lazyEffect: "fade",
        autoHeight: !1,
        jsonPath: !1,
        jsonSuccess: !1,
        dragBeforeAnimFinish: !0,
        mouseDrag: !0,
        touchDrag: !0,
        addClassActive: !1,
        transitionStyle: !1,
        beforeUpdate: !1,
        afterUpdate: !1,
        beforeInit: !1,
        afterInit: !1,
        beforeMove: !1,
        afterMove: !1,
        afterAction: !1,
        startDragging: !1,
        afterLazyLoad: !1
    }
})(jQuery, window, document);


/*jshint browser:true */
/*!
 * FitVids 1.1
 *
 * Copyright 2013, Chris Coyier - https://css-tricks.com + Dave Rupert - https://daverupert.com
 * Credit to Thierry Koblentz - https://www.alistapart.com/articles/creating-intrinsic-ratios-for-video/
 * Released under the WTFPL license - https://sam.zoy.org/wtfpl/
 *
 */

;
(function($) {
    'use strict';
    $.fn.fitVids = function(options) {
        var settings = { customSelector: null, ignore: null };
        if (!document.getElementById('fit-vids-style')) {
            var head = document.head || document.getElementsByTagName('head')[0];
            var css = '.fluid-width-video-wrapper{width:100%;position:relative;padding:0;}.fluid-width-video-wrapper iframe,.fluid-width-video-wrapper object,.fluid-width-video-wrapper embed {position:absolute;top:0;left:0;width:100%;height:100%;}';
            var div = document.createElement("div");
            div.innerHTML = '<p>x</p><style id="fit-vids-style">' + css + '</style>';
            head.appendChild(div.childNodes[1])
        }
        if (options) { $.extend(settings, options) }
        return this.each(function() {
            var selectors = ['iframe[src*="player.vimeo.com"]', 'iframe[src*="youtube.com"]', 'iframe[src*="youtube-nocookie.com"]', 'iframe[src*="kickstarter.com"][src*="video.html"]', 'object', 'embed'];
            if (settings.customSelector) { selectors.push(settings.customSelector) }
            var ignoreList = '.fitvidsignore';
            if (settings.ignore) { ignoreList = ignoreList + ', ' + settings.ignore }
            var $allVideos = $(this).find(selectors.join(','));
            $allVideos = $allVideos.not('object object');
            $allVideos = $allVideos.not(ignoreList);
            $allVideos.each(function() {
                var $this = $(this);
                if ($this.parents(ignoreList).length > 0) { return }
                if (this.tagName.toLowerCase() === 'embed' && $this.parent('object').length || $this.parent('.fluid-width-video-wrapper').length) { return }
                if ((!$this.css('height') && !$this.css('width')) && (isNaN($this.attr('height')) || isNaN($this.attr('width')))) {
                    $this.attr('height', 9);
                    $this.attr('width', 16)
                }
                var height = (this.tagName.toLowerCase() === 'object' || ($this.attr('height') && !isNaN(parseInt($this.attr('height'), 10)))) ? parseInt($this.attr('height'), 10) : $this.height(),
                    width = !isNaN(parseInt($this.attr('width'), 10)) ? parseInt($this.attr('width'), 10) : $this.width(),
                    aspectRatio = height / width;
                if (!$this.attr('name')) {
                    var videoName = 'fitvid' + $.fn.fitVids._count;
                    $this.attr('name', videoName);
                    $.fn.fitVids._count++
                }
                $this.wrap('<div class="fluid-width-video-wrapper"></div>').parent('.fluid-width-video-wrapper').css('padding-top', (aspectRatio * 100) + '%');
                $this.removeAttr('height').removeAttr('width')
            })
        })
    };
    $.fn.fitVids._count = 0
})(window.jQuery || window.Zepto);


/*!
 * Lightbox v2.8.2
 * by Lokesh Dhakar
 *
 * More info:
 * https://lokeshdhakar.com/projects/lightbox2/
 *
 * Copyright 2007, 2015 Lokesh Dhakar
 * Released under the MIT license
 * https://github.com/lokesh/lightbox2/blob/master/LICENSE
 */
! function(a, b) { "function" == typeof define && define.amd ? define(["jquery"], b) : "object" == typeof exports ? module.exports = b(require("jquery")) : a.lightbox = b(a.jQuery) }(this, function(a) {
    function b(b) { this.album = [], this.currentImageIndex = void 0, this.init(), this.options = a.extend({}, this.constructor.defaults), this.option(b) }
    return b.defaults = { albumLabel: "Image %1 of %2", alwaysShowNavOnTouchDevices: !1, fadeDuration: 500, fitImagesInViewport: !0, positionFromTop: 50, resizeDuration: 700, showImageNumberLabel: !0, wrapAround: !1, disableScrolling: !1 }, b.prototype.option = function(b) { a.extend(this.options, b) }, b.prototype.imageCountLabel = function(a, b) { return this.options.albumLabel.replace(/%1/g, a).replace(/%2/g, b) }, b.prototype.init = function() { this.enable(), this.build() }, b.prototype.enable = function() {
        var b = this;
        a("body").on("click", "a[rel^=lightbox], area[rel^=lightbox], a[data-lightbox], area[data-lightbox]", function(c) { return b.start(a(c.currentTarget)), !1 })
    }, b.prototype.build = function() {
        var b = this;
        a('<div id="lightboxOverlay" class="lightboxOverlay"></div><div id="lightbox" class="lightbox"><div class="lb-outerContainer"><div class="lb-container"><img class="lb-image" src="data:image/gif;base64,R0lGODlhAQABAIAAAP///wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==" /><div class="lb-nav"><a class="lb-prev" href="" ></a><a class="lb-next" href="" ></a></div><div class="lb-loader"><a class="lb-cancel"></a></div></div></div><div class="lb-dataContainer"><div class="lb-data"><div class="lb-details"><span class="lb-caption"></span><span class="lb-number"></span></div><div class="lb-closeContainer"><a class="lb-close"></a></div></div></div></div>').appendTo(a("body")), this.$lightbox = a("#lightbox"), this.$overlay = a("#lightboxOverlay"), this.$outerContainer = this.$lightbox.find(".lb-outerContainer"), this.$container = this.$lightbox.find(".lb-container"), this.containerTopPadding = parseInt(this.$container.css("padding-top"), 10), this.containerRightPadding = parseInt(this.$container.css("padding-right"), 10), this.containerBottomPadding = parseInt(this.$container.css("padding-bottom"), 10), this.containerLeftPadding = parseInt(this.$container.css("padding-left"), 10), this.$overlay.hide().on("click", function() { return b.end(), !1 }), this.$lightbox.hide().on("click", function(c) { return "lightbox" === a(c.target).attr("id") && b.end(), !1 }), this.$outerContainer.on("click", function(c) { return "lightbox" === a(c.target).attr("id") && b.end(), !1 }), this.$lightbox.find(".lb-prev").on("click", function() { return 0 === b.currentImageIndex ? b.changeImage(b.album.length - 1) : b.changeImage(b.currentImageIndex - 1), !1 }), this.$lightbox.find(".lb-next").on("click", function() { return b.currentImageIndex === b.album.length - 1 ? b.changeImage(0) : b.changeImage(b.currentImageIndex + 1), !1 }), this.$lightbox.find(".lb-loader, .lb-close").on("click", function() { return b.end(), !1 })
    }, b.prototype.start = function(b) {
        function c(a) { d.album.push({ link: a.attr("href"), title: a.attr("data-title") || a.attr("title") }) }
        var d = this,
            e = a(window);
        e.on("resize", a.proxy(this.sizeOverlay, this)), a("select, object, embed").css({ visibility: "hidden" }), this.sizeOverlay(), this.album = [];
        var f, g = 0,
            h = b.attr("data-lightbox");
        if (h) { f = a(b.prop("tagName") + '[data-lightbox="' + h + '"]'); for (var i = 0; i < f.length; i = ++i) c(a(f[i])), f[i] === b[0] && (g = i) } else if ("lightbox" === b.attr("rel")) c(b);
        else { f = a(b.prop("tagName") + '[rel="' + b.attr("rel") + '"]'); for (var j = 0; j < f.length; j = ++j) c(a(f[j])), f[j] === b[0] && (g = j) }
        var k = e.scrollTop() + this.options.positionFromTop,
            l = e.scrollLeft();
        this.$lightbox.css({ top: k + "px", left: l + "px" }).fadeIn(this.options.fadeDuration), this.options.disableScrolling && a("body").addClass("lb-disable-scrolling"), this.changeImage(g)
    }, b.prototype.changeImage = function(b) {
        var c = this;
        this.disableKeyboardNav();
        var d = this.$lightbox.find(".lb-image");
        this.$overlay.fadeIn(this.options.fadeDuration), a(".lb-loader").fadeIn("slow"), this.$lightbox.find(".lb-image, .lb-nav, .lb-prev, .lb-next, .lb-dataContainer, .lb-numbers, .lb-caption").hide(), this.$outerContainer.addClass("animating");
        var e = new Image;
        e.onload = function() {
            var f, g, h, i, j, k, l;
            d.attr("src", c.album[b].link), f = a(e), d.width(e.width), d.height(e.height), c.options.fitImagesInViewport && (l = a(window).width(), k = a(window).height(), j = l - c.containerLeftPadding - c.containerRightPadding - 20, i = k - c.containerTopPadding - c.containerBottomPadding - 120, c.options.maxWidth && c.options.maxWidth < j && (j = c.options.maxWidth), c.options.maxHeight && c.options.maxHeight < j && (i = c.options.maxHeight), (e.width > j || e.height > i) && (e.width / j > e.height / i ? (h = j, g = parseInt(e.height / (e.width / h), 10), d.width(h), d.height(g)) : (g = i, h = parseInt(e.width / (e.height / g), 10), d.width(h), d.height(g)))), c.sizeContainer(d.width(), d.height())
        }, e.src = this.album[b].link, this.currentImageIndex = b
    }, b.prototype.sizeOverlay = function() { this.$overlay.width(a(document).width()).height(a(document).height()) }, b.prototype.sizeContainer = function(a, b) {
        function c() { d.$lightbox.find(".lb-dataContainer").width(g), d.$lightbox.find(".lb-prevLink").height(h), d.$lightbox.find(".lb-nextLink").height(h), d.showImage() }
        var d = this,
            e = this.$outerContainer.outerWidth(),
            f = this.$outerContainer.outerHeight(),
            g = a + this.containerLeftPadding + this.containerRightPadding,
            h = b + this.containerTopPadding + this.containerBottomPadding;
        e !== g || f !== h ? this.$outerContainer.animate({ width: g, height: h }, this.options.resizeDuration, "swing", function() { c() }) : c()
    }, b.prototype.showImage = function() { this.$lightbox.find(".lb-loader").stop(!0).hide(), this.$lightbox.find(".lb-image").fadeIn("slow"), this.updateNav(), this.updateDetails(), this.preloadNeighboringImages(), this.enableKeyboardNav() }, b.prototype.updateNav = function() {
        var a = !1;
        try { document.createEvent("TouchEvent"), a = this.options.alwaysShowNavOnTouchDevices ? !0 : !1 } catch (b) {}
        this.$lightbox.find(".lb-nav").show(), this.album.length > 1 && (this.options.wrapAround ? (a && this.$lightbox.find(".lb-prev, .lb-next").css("opacity", "1"), this.$lightbox.find(".lb-prev, .lb-next").show()) : (this.currentImageIndex > 0 && (this.$lightbox.find(".lb-prev").show(), a && this.$lightbox.find(".lb-prev").css("opacity", "1")), this.currentImageIndex < this.album.length - 1 && (this.$lightbox.find(".lb-next").show(), a && this.$lightbox.find(".lb-next").css("opacity", "1"))))
    }, b.prototype.updateDetails = function() {
        var b = this;
        if ("undefined" != typeof this.album[this.currentImageIndex].title && "" !== this.album[this.currentImageIndex].title && this.$lightbox.find(".lb-caption").html(this.album[this.currentImageIndex].title).fadeIn("fast").find("a").on("click", function(b) { void 0 !== a(this).attr("target") ? window.open(a(this).attr("href"), a(this).attr("target")) : location.href = a(this).attr("href") }), this.album.length > 1 && this.options.showImageNumberLabel) {
            var c = this.imageCountLabel(this.currentImageIndex + 1, this.album.length);
            this.$lightbox.find(".lb-number").text(c).fadeIn("fast")
        } else this.$lightbox.find(".lb-number").hide();
        this.$outerContainer.removeClass("animating"), this.$lightbox.find(".lb-dataContainer").fadeIn(this.options.resizeDuration, function() { return b.sizeOverlay() })
    }, b.prototype.preloadNeighboringImages = function() {
        if (this.album.length > this.currentImageIndex + 1) {
            var a = new Image;
            a.src = this.album[this.currentImageIndex + 1].link
        }
        if (this.currentImageIndex > 0) {
            var b = new Image;
            b.src = this.album[this.currentImageIndex - 1].link
        }
    }, b.prototype.enableKeyboardNav = function() { a(document).on("keyup.keyboard", a.proxy(this.keyboardAction, this)) }, b.prototype.disableKeyboardNav = function() { a(document).off(".keyboard") }, b.prototype.keyboardAction = function(a) {
        var b = 27,
            c = 37,
            d = 39,
            e = a.keyCode,
            f = String.fromCharCode(e).toLowerCase();
        e === b || f.match(/x|o|c/) ? this.end() : "p" === f || e === c ? 0 !== this.currentImageIndex ? this.changeImage(this.currentImageIndex - 1) : this.options.wrapAround && this.album.length > 1 && this.changeImage(this.album.length - 1) : ("n" === f || e === d) && (this.currentImageIndex !== this.album.length - 1 ? this.changeImage(this.currentImageIndex + 1) : this.options.wrapAround && this.album.length > 1 && this.changeImage(0))
    }, b.prototype.end = function() { this.disableKeyboardNav(), a(window).off("resize", this.sizeOverlay), this.$lightbox.fadeOut(this.options.fadeDuration), this.$overlay.fadeOut(this.options.fadeDuration), a("select, object, embed").css({ visibility: "visible" }), this.options.disableScrolling && a("body").removeClass("lb-disable-scrolling") }, new b
});
//# sourceMappingURL=lightbox.min.map

/*!
 * Bootstrap-select v1.11.2 (https://silviomoreto.github.io/bootstrap-select)
 *
 * Copyright 2013-2016 bootstrap-select
 * Licensed under MIT (https://github.com/silviomoreto/bootstrap-select/blob/master/LICENSE)
 */
! function(a) {
    "use strict";

    function d(b) { var c = [{ re: /[\xC0-\xC6]/g, ch: "A" }, { re: /[\xE0-\xE6]/g, ch: "a" }, { re: /[\xC8-\xCB]/g, ch: "E" }, { re: /[\xE8-\xEB]/g, ch: "e" }, { re: /[\xCC-\xCF]/g, ch: "I" }, { re: /[\xEC-\xEF]/g, ch: "i" }, { re: /[\xD2-\xD6]/g, ch: "O" }, { re: /[\xF2-\xF6]/g, ch: "o" }, { re: /[\xD9-\xDC]/g, ch: "U" }, { re: /[\xF9-\xFC]/g, ch: "u" }, { re: /[\xC7-\xE7]/g, ch: "c" }, { re: /[\xD1]/g, ch: "N" }, { re: /[\xF1]/g, ch: "n" }]; return a.each(c, function() { b = b.replace(this.re, this.ch) }), b }

    function k(b, c) {
        var d = arguments,
            e = b,
            f = c;
        [].shift.apply(d);
        var g, h = this.each(function() {
            var b = a(this);
            if (b.is("select")) {
                var c = b.data("selectpicker"),
                    h = "object" == typeof e && e;
                if (c) {
                    if (h)
                        for (var k in h) h.hasOwnProperty(k) && (c.options[k] = h[k])
                } else {
                    var i = a.extend({}, j.DEFAULTS, a.fn.selectpicker.defaults || {}, b.data(), h);
                    i.template = a.extend({}, j.DEFAULTS.template, a.fn.selectpicker.defaults ? a.fn.selectpicker.defaults.template : {}, b.data().template, h.template), b.data("selectpicker", c = new j(this, i, f))
                }
                "string" == typeof e && (g = c[e] instanceof Function ? c[e].apply(c, d) : c.options[e])
            }
        });
        return "undefined" != typeof g ? g : h
    }
    String.prototype.includes || ! function() {
        var a = {}.toString,
            b = function() {
                try {
                    var a = {},
                        b = Object.defineProperty,
                        c = b(a, a, a) && b
                } catch (a) {}
                return c
            }(),
            c = "".indexOf,
            d = function(b) {
                if (null == this) throw new TypeError;
                var d = String(this);
                if (b && "[object RegExp]" == a.call(b)) throw new TypeError;
                var e = d.length,
                    f = String(b),
                    g = f.length,
                    h = arguments.length > 1 ? arguments[1] : void 0,
                    i = h ? Number(h) : 0;
                i != i && (i = 0);
                var j = Math.min(Math.max(i, 0), e);
                return !(g + j > e) && c.call(d, f, i) != -1
            };
        b ? b(String.prototype, "includes", { value: d, configurable: !0, writable: !0 }) : String.prototype.includes = d
    }(), String.prototype.startsWith || ! function() {
        var a = function() {
                try {
                    var a = {},
                        b = Object.defineProperty,
                        c = b(a, a, a) && b
                } catch (a) {}
                return c
            }(),
            b = {}.toString,
            c = function(a) {
                if (null == this) throw new TypeError;
                var c = String(this);
                if (a && "[object RegExp]" == b.call(a)) throw new TypeError;
                var d = c.length,
                    e = String(a),
                    f = e.length,
                    g = arguments.length > 1 ? arguments[1] : void 0,
                    h = g ? Number(g) : 0;
                h != h && (h = 0);
                var i = Math.min(Math.max(h, 0), d);
                if (f + i > d) return !1;
                for (var j = -1; ++j < f;)
                    if (c.charCodeAt(i + j) != e.charCodeAt(j)) return !1;
                return !0
            };
        a ? a(String.prototype, "startsWith", { value: c, configurable: !0, writable: !0 }) : String.prototype.startsWith = c
    }(), Object.keys || (Object.keys = function(a, b, c) { c = []; for (b in a) c.hasOwnProperty.call(a, b) && c.push(b); return c });
    var b = { useDefault: !1, _set: a.valHooks.select.set };
    a.valHooks.select.set = function(c, d) { return d && !b.useDefault && a(c).data("selected", !0), b._set.apply(this, arguments) };
    var c = null;
    a.fn.triggerNative = function(a) {
        var c, b = this[0];
        b.dispatchEvent ? ("function" == typeof Event ? c = new Event(a, { bubbles: !0 }) : (c = document.createEvent("Event"), c.initEvent(a, !0, !1)), b.dispatchEvent(c)) : b.fireEvent ? (c = document.createEventObject(), c.eventType = a, b.fireEvent("on" + a, c)) : this.trigger(a)
    }, a.expr.pseudos.icontains = function(b, c, d) {
        var e = a(b),
            f = (e.data("tokens") || e.text()).toString().toUpperCase();
        return f.includes(d[3].toUpperCase())
    }, a.expr.pseudos.ibegins = function(b, c, d) {
        var e = a(b),
            f = (e.data("tokens") || e.text()).toString().toUpperCase();
        return f.startsWith(d[3].toUpperCase())
    }, a.expr.pseudos.aicontains = function(b, c, d) {
        var e = a(b),
            f = (e.data("tokens") || e.data("normalizedText") || e.text()).toString().toUpperCase();
        return f.includes(d[3].toUpperCase())
    }, a.expr.pseudos.aibegins = function(b, c, d) {
        var e = a(b),
            f = (e.data("tokens") || e.data("normalizedText") || e.text()).toString().toUpperCase();
        return f.startsWith(d[3].toUpperCase())
    };
    var e = { "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#x27;", "`": "&#x60;" },
        f = { "&amp;": "&", "&lt;": "<", "&gt;": ">", "&quot;": '"', "&#x27;": "'", "&#x60;": "`" },
        g = function(a) {
            var b = function(b) { return a[b] },
                c = "(?:" + Object.keys(a).join("|") + ")",
                d = RegExp(c),
                e = RegExp(c, "g");
            return function(a) { return a = null == a ? "" : "" + a, d.test(a) ? a.replace(e, b) : a }
        },
        h = g(e),
        i = g(f),
        j = function(c, d, e) { b.useDefault || (a.valHooks.select.set = b._set, b.useDefault = !0), e && (e.stopPropagation(), e.preventDefault()), this.$element = a(c), this.$newElement = null, this.$button = null, this.$menu = null, this.$lis = null, this.options = d, null === this.options.title && (this.options.title = this.$element.attr("title")); var f = this.options.windowPadding; "number" == typeof f && (this.options.windowPadding = [f, f, f, f]), this.val = j.prototype.val, this.render = j.prototype.render, this.refresh = j.prototype.refresh, this.setStyle = j.prototype.setStyle, this.selectAll = j.prototype.selectAll, this.deselectAll = j.prototype.deselectAll, this.destroy = j.prototype.destroy, this.remove = j.prototype.remove, this.show = j.prototype.show, this.hide = j.prototype.hide, this.init() };
    j.VERSION = "1.11.2", j.DEFAULTS = { noneSelectedText: "Nothing selected", noneResultsText: "No results matched {0}", countSelectedText: function(a, b) { return 1 == a ? "{0} item selected" : "{0} items selected" }, maxOptionsText: function(a, b) { return [1 == a ? "Limit reached ({n} item max)" : "Limit reached ({n} items max)", 1 == b ? "Group limit reached ({n} item max)" : "Group limit reached ({n} items max)"] }, selectAllText: "Select All", deselectAllText: "Deselect All", doneButton: !1, doneButtonText: "Close", multipleSeparator: ", ", styleBase: "btn", style: "btn-default", size: "auto", title: null, selectedTextFormat: "values", width: !1, container: !1, hideDisabled: !1, showSubtext: !1, showIcon: !0, showContent: !0, dropupAuto: !1, header: !1, liveSearch: !1, liveSearchPlaceholder: null, liveSearchNormalize: !1, liveSearchStyle: "contains", actionsBox: !1, iconBase: "glyphicon", tickIcon: "glyphicon-ok", showTick: !1, template: { caret: '<span class="caret"></span>' }, maxOptions: !1, mobile: !1, selectOnTab: !1, dropdownAlignRight: !1, windowPadding: 0 }, j.prototype = {
        constructor: j,
        init: function() {
            var b = this,
                c = this.$element.attr("id");
            this.$element.addClass("bs-select-hidden"), this.liObj = {}, this.multiple = this.$element.prop("multiple"), this.autofocus = this.$element.prop("autofocus"), this.$newElement = this.createView(), this.$element.after(this.$newElement).appendTo(this.$newElement), this.$button = this.$newElement.children("button"), this.$menu = this.$newElement.children(".dropdown-menu"), this.$menuInner = this.$menu.children(".inner"), this.$searchbox = this.$menu.find("input"), this.$element.removeClass("bs-select-hidden"), this.options.dropdownAlignRight === !0 && this.$menu.addClass("dropdown-menu-right"), "undefined" != typeof c && (this.$button.attr("data-id", c), a('label[for="' + c + '"]').click(function(a) { a.preventDefault(), b.$button.focus() })), this.checkDisabled(), this.clickListener(), this.options.liveSearch && this.liveSearchListener(), this.render(), this.setStyle(), this.setWidth(), this.options.container && this.selectPosition(), this.$menu.data("this", this), this.$newElement.data("this", this), this.options.mobile && this.mobile(), this.$newElement.on({ "hide.bs.dropdown": function(a) { b.$menuInner.attr("aria-expanded", !1), b.$element.trigger("hide.bs.select", a) }, "hidden.bs.dropdown": function(a) { b.$element.trigger("hidden.bs.select", a) }, "show.bs.dropdown": function(a) { b.$menuInner.attr("aria-expanded", !0), b.$element.trigger("show.bs.select", a) }, "shown.bs.dropdown": function(a) { b.$element.trigger("shown.bs.select", a) } }), b.$element[0].hasAttribute("required") && this.$element.on("invalid", function() { b.$button.addClass("bs-invalid").focus(), b.$element.on({ "focus.bs.select": function() { b.$button.focus(), b.$element.off("focus.bs.select") }, "shown.bs.select": function() { b.$element.val(b.$element.val()).off("shown.bs.select") }, "rendered.bs.select": function() { this.validity.valid && b.$button.removeClass("bs-invalid"), b.$element.off("rendered.bs.select") } }) }), setTimeout(function() { b.$element.trigger("loaded.bs.select") })
        },
        createDropdown: function() {
            var b = this.multiple || this.options.showTick ? " show-tick" : "",
                c = this.$element.parent().hasClass("input-group") ? " input-group-btn" : "",
                d = this.autofocus ? " autofocus" : "",
                e = this.options.header ? '<div class="popover-title"><button type="button" class="close" aria-hidden="true">&times;</button>' + this.options.header + "</div>" : "",
                f = this.options.liveSearch ? '<div class="bs-searchbox"><input type="text" class="form-control" autocomplete="off"' + (null === this.options.liveSearchPlaceholder ? "" : ' placeholder="' + h(this.options.liveSearchPlaceholder) + '"') + ' role="textbox" aria-label="Search"></div>' : "",
                g = this.multiple && this.options.actionsBox ? '<div class="bs-actionsbox"><div class="btn-group btn-group-sm btn-block"><button type="button" class="actions-btn bs-select-all btn btn-default">' + this.options.selectAllText + '</button><button type="button" class="actions-btn bs-deselect-all btn btn-default">' + this.options.deselectAllText + "</button></div></div>" : "",
                i = this.multiple && this.options.doneButton ? '<div class="bs-donebutton"><div class="btn-group btn-block"><button type="button" class="btn btn-sm btn-default">' + this.options.doneButtonText + "</button></div></div>" : "",
                j = '<div class="btn-group bootstrap-select' + b + c + '"><button type="button" class="' + this.options.styleBase + ' dropdown-toggle" data-toggle="dropdown"' + d + ' role="button"><span class="filter-option pull-left"></span>&nbsp;<span class="bs-caret">' + this.options.template.caret + '</span></button><div class="dropdown-menu open" role="combobox">' + e + f + g + '<ul class="dropdown-menu inner" role="listbox" aria-expanded="false"></ul>' + i + "</div></div>";
            return a(j)
        },
        createView: function() {
            var a = this.createDropdown(),
                b = this.createLi();
            return a.find("ul")[0].innerHTML = b, a
        },
        reloadLi: function() {
            this.destroyLi();
            var a = this.createLi();
            this.$menuInner[0].innerHTML = a
        },
        destroyLi: function() { this.$menu.find("li").remove() },
        createLi: function() {
            var b = this,
                c = [],
                e = 0,
                f = document.createElement("option"),
                g = -1,
                i = function(a, b, c, d) { return "<li" + ("undefined" != typeof c & "" !== c ? ' class="' + c + '"' : "") + ("undefined" != typeof b & null !== b ? ' data-original-index="' + b + '"' : "") + ("undefined" != typeof d & null !== d ? 'data-optgroup="' + d + '"' : "") + ">" + a + "</li>" },
                j = function(c, e, f, g) { return '<a tabindex="0"' + ("undefined" != typeof e ? ' class="' + e + '"' : "") + (f ? ' style="' + f + '"' : "") + (b.options.liveSearchNormalize ? ' data-normalized-text="' + d(h(a(c).html())) + '"' : "") + ("undefined" != typeof g || null !== g ? ' data-tokens="' + g + '"' : "") + ' role="option">' + c + '<span class="' + b.options.iconBase + " " + b.options.tickIcon + ' check-mark"></span></a>' };
            if (this.options.title && !this.multiple && (g--, !this.$element.find(".bs-title-option").length)) {
                var k = this.$element[0];
                f.className = "bs-title-option", f.appendChild(document.createTextNode(this.options.title)), f.value = "", k.insertBefore(f, k.firstChild);
                var l = a(k.options[k.selectedIndex]);
                void 0 === l.attr("selected") && void 0 === this.$element.data("selected") && (f.selected = !0)
            }
            return this.$element.find("option").each(function(d) {
                var f = a(this);
                if (g++, !f.hasClass("bs-title-option")) {
                    var k = this.className || "",
                        l = this.style.cssText,
                        m = f.data("content") ? f.data("content") : f.html(),
                        n = f.data("tokens") ? f.data("tokens") : null,
                        o = "undefined" != typeof f.data("subtext") ? '<small class="text-muted">' + f.data("subtext") + "</small>" : "",
                        p = "undefined" != typeof f.data("icon") ? '<span class="' + b.options.iconBase + " " + f.data("icon") + '"></span> ' : "",
                        q = f.parent(),
                        r = "OPTGROUP" === q[0].tagName,
                        s = r && q[0].disabled,
                        t = this.disabled || s;
                    if ("" !== p && t && (p = "<span>" + p + "</span>"), b.options.hideDisabled && (t && !r || s)) return void g--;
                    if (f.data("content") || (m = p + '<span class="text">' + m + o + "</span>"), r && f.data("divider") !== !0) {
                        if (b.options.hideDisabled && t) {
                            if (void 0 === q.data("allOptionsDisabled")) {
                                var u = q.children();
                                q.data("allOptionsDisabled", u.filter(":disabled").length === u.length)
                            }
                            if (q.data("allOptionsDisabled")) return void g--
                        }
                        var v = " " + q[0].className || "";
                        if (0 === f.index()) {
                            e += 1;
                            var w = q[0].label,
                                x = "undefined" != typeof q.data("subtext") ? '<small class="text-muted">' + q.data("subtext") + "</small>" : "",
                                y = q.data("icon") ? '<span class="' + b.options.iconBase + " " + q.data("icon") + '"></span> ' : "";
                            w = y + '<span class="text">' + h(w) + x + "</span>", 0 !== d && c.length > 0 && (g++, c.push(i("", null, "divider", e + "div"))), g++, c.push(i(w, null, "dropdown-header" + v, e))
                        }
                        if (b.options.hideDisabled && t) return void g--;
                        c.push(i(j(m, "opt " + k + v, l, n), d, "", e))
                    } else if (f.data("divider") === !0) c.push(i("", d, "divider"));
                    else if (f.data("hidden") === !0) c.push(i(j(m, k, l, n), d, "hidden is-hidden"));
                    else {
                        var z = this.previousElementSibling && "OPTGROUP" === this.previousElementSibling.tagName;
                        if (!z && b.options.hideDisabled)
                            for (var A = a(this).prevAll(), B = 0; B < A.length; B++)
                                if ("OPTGROUP" === A[B].tagName) {
                                    for (var C = 0, D = 0; D < B; D++) {
                                        var E = A[D];
                                        (E.disabled || a(E).data("hidden") === !0) && C++
                                    }
                                    C === B && (z = !0);
                                    break
                                }
                        z && (g++, c.push(i("", null, "divider", e + "div"))), c.push(i(j(m, k, l, n), d))
                    }
                    b.liObj[d] = g
                }
            }), this.multiple || 0 !== this.$element.find("option:selected").length || this.options.title || this.$element.find("option").eq(0).prop("selected", !0).attr("selected", "selected"), c.join("")
        },
        findLis: function() { return null == this.$lis && (this.$lis = this.$menu.find("li")), this.$lis },
        render: function(b) {
            var d, c = this;
            b !== !1 && this.$element.find("option").each(function(a) {
                var b = c.findLis().eq(c.liObj[a]);
                c.setDisabled(a, this.disabled || "OPTGROUP" === this.parentNode.tagName && this.parentNode.disabled, b), c.setSelected(a, this.selected, b)
            }), this.togglePlaceholder(), this.tabIndex();
            var e = this.$element.find("option").map(function() {
                    if (this.selected) {
                        if (c.options.hideDisabled && (this.disabled || "OPTGROUP" === this.parentNode.tagName && this.parentNode.disabled)) return;
                        var e, b = a(this),
                            d = b.data("icon") && c.options.showIcon ? '<i class="' + c.options.iconBase + " " + b.data("icon") + '"></i> ' : "";
                        return e = c.options.showSubtext && b.data("subtext") && !c.multiple ? ' <small class="text-muted">' + b.data("subtext") + "</small>" : "", "undefined" != typeof b.attr("title") ? b.attr("title") : b.data("content") && c.options.showContent ? b.data("content").toString() : d + b.html() + e
                    }
                }).toArray(),
                f = this.multiple ? e.join(this.options.multipleSeparator) : e[0];
            if (this.multiple && this.options.selectedTextFormat.indexOf("count") > -1) {
                var g = this.options.selectedTextFormat.split(">");
                if (g.length > 1 && e.length > g[1] || 1 == g.length && e.length >= 2) {
                    d = this.options.hideDisabled ? ", [disabled]" : "";
                    var h = this.$element.find("option").not('[data-divider="true"], [data-hidden="true"]' + d).length,
                        j = "function" == typeof this.options.countSelectedText ? this.options.countSelectedText(e.length, h) : this.options.countSelectedText;
                    f = j.replace("{0}", e.length.toString()).replace("{1}", h.toString())
                }
            }
            void 0 == this.options.title && (this.options.title = this.$element.attr("title")), "static" == this.options.selectedTextFormat && (f = this.options.title), f || (f = "undefined" != typeof this.options.title ? this.options.title : this.options.noneSelectedText), this.$button.attr("title", i(a.trim(f.replace(/<[^>]*>?/g, "")))), this.$button.children(".filter-option").html(f), this.$element.trigger("rendered.bs.select")
        },
        setStyle: function(a, b) { this.$element.attr("class") && this.$newElement.addClass(this.$element.attr("class").replace(/selectpicker|mobile-device|bs-select-hidden|validate\[.*\]/gi, "")); var c = a ? a : this.options.style; "add" == b ? this.$button.addClass(c) : "remove" == b ? this.$button.removeClass(c) : (this.$button.removeClass(this.options.style), this.$button.addClass(c)) },
        liHeight: function(b) {
            if (b || this.options.size !== !1 && !this.sizeInfo) {
                var c = document.createElement("div"),
                    d = document.createElement("div"),
                    e = document.createElement("ul"),
                    f = document.createElement("li"),
                    g = document.createElement("li"),
                    h = document.createElement("a"),
                    i = document.createElement("span"),
                    j = this.options.header && this.$menu.find(".popover-title").length > 0 ? this.$menu.find(".popover-title")[0].cloneNode(!0) : null,
                    k = this.options.liveSearch ? document.createElement("div") : null,
                    l = this.options.actionsBox && this.multiple && this.$menu.find(".bs-actionsbox").length > 0 ? this.$menu.find(".bs-actionsbox")[0].cloneNode(!0) : null,
                    m = this.options.doneButton && this.multiple && this.$menu.find(".bs-donebutton").length > 0 ? this.$menu.find(".bs-donebutton")[0].cloneNode(!0) : null;
                if (i.className = "text", c.className = this.$menu[0].parentNode.className + " open", d.className = "dropdown-menu open", e.className = "dropdown-menu inner", f.className = "divider", i.appendChild(document.createTextNode("Inner text")), h.appendChild(i), g.appendChild(h), e.appendChild(g), e.appendChild(f), j && d.appendChild(j), k) {
                    var n = document.createElement("span");
                    k.className = "bs-searchbox", n.className = "form-control", k.appendChild(n), d.appendChild(k)
                }
                l && d.appendChild(l), d.appendChild(e), m && d.appendChild(m), c.appendChild(d), document.body.appendChild(c);
                var o = h.offsetHeight,
                    p = j ? j.offsetHeight : 0,
                    q = k ? k.offsetHeight : 0,
                    r = l ? l.offsetHeight : 0,
                    s = m ? m.offsetHeight : 0,
                    t = a(f).outerHeight(!0),
                    u = "function" == typeof getComputedStyle && getComputedStyle(d),
                    v = u ? null : a(d),
                    w = { vert: parseInt(u ? u.paddingTop : v.css("paddingTop")) + parseInt(u ? u.paddingBottom : v.css("paddingBottom")) + parseInt(u ? u.borderTopWidth : v.css("borderTopWidth")) + parseInt(u ? u.borderBottomWidth : v.css("borderBottomWidth")), horiz: parseInt(u ? u.paddingLeft : v.css("paddingLeft")) + parseInt(u ? u.paddingRight : v.css("paddingRight")) + parseInt(u ? u.borderLeftWidth : v.css("borderLeftWidth")) + parseInt(u ? u.borderRightWidth : v.css("borderRightWidth")) },
                    x = { vert: w.vert + parseInt(u ? u.marginTop : v.css("marginTop")) + parseInt(u ? u.marginBottom : v.css("marginBottom")) + 2, horiz: w.horiz + parseInt(u ? u.marginLeft : v.css("marginLeft")) + parseInt(u ? u.marginRight : v.css("marginRight")) + 2 };
                document.body.removeChild(c), this.sizeInfo = { liHeight: o, headerHeight: p, searchHeight: q, actionsHeight: r, doneButtonHeight: s, dividerHeight: t, menuPadding: w, menuExtras: x }
            }
        },
        setSize: function() {
            if (this.findLis(), this.liHeight(), this.options.header && this.$menu.css("padding-top", 0), this.options.size !== !1) {
                var q, r, s, t, u, v, w, x, b = this,
                    c = this.$menu,
                    d = this.$menuInner,
                    e = a(window),
                    f = this.$newElement[0].offsetHeight,
                    g = this.$newElement[0].offsetWidth,
                    h = this.sizeInfo.liHeight,
                    i = this.sizeInfo.headerHeight,
                    j = this.sizeInfo.searchHeight,
                    k = this.sizeInfo.actionsHeight,
                    l = this.sizeInfo.doneButtonHeight,
                    m = this.sizeInfo.dividerHeight,
                    n = this.sizeInfo.menuPadding,
                    o = this.sizeInfo.menuExtras,
                    p = this.options.hideDisabled ? ".disabled" : "",
                    y = function() {
                        var h, c = b.$newElement.offset(),
                            d = a(b.options.container);
                        b.options.container && !d.is("body") ? (h = d.offset(), h.top += parseInt(d.css("borderTopWidth")), h.left += parseInt(d.css("borderLeftWidth"))) : h = { top: 0, left: 0 };
                        var i = b.options.windowPadding;
                        u = c.top - h.top - e.scrollTop(), v = e.height() - u - f - h.top - i[2], w = c.left - h.left - e.scrollLeft(), x = e.width() - w - g - h.left - i[1], u -= i[0], w -= i[3]
                    };
                if (y(), "auto" === this.options.size) {
                    var z = function() {
                        var e, f = function(b, c) { return function(d) { return c ? d.classList ? d.classList.contains(b) : a(d).hasClass(b) : !(d.classList ? d.classList.contains(b) : a(d).hasClass(b)) } },
                            m = b.$menuInner[0].getElementsByTagName("li"),
                            p = Array.prototype.filter ? Array.prototype.filter.call(m, f("hidden", !1)) : b.$lis.not(".hidden"),
                            z = Array.prototype.filter ? Array.prototype.filter.call(p, f("dropdown-header", !0)) : p.filter(".dropdown-header");
                        y(), q = v - o.vert, r = x - o.horiz, b.options.container ? (c.data("height") || c.data("height", c.height()), s = c.data("height"), c.data("width") || c.data("width", c.width()), t = c.data("width")) : (s = c.height(), t = c.width()), b.options.dropupAuto && b.$newElement.toggleClass("dropup", u > v && q - o.vert < s), b.$newElement.hasClass("dropup") && (q = u - o.vert), "auto" === b.options.dropdownAlignRight && c.toggleClass("dropdown-menu-right", w > x && r - o.horiz < t - g), e = p.length + z.length > 3 ? 3 * h + o.vert - 2 : 0, c.css({ "max-height": q + "px", overflow: "hidden", "min-height": e + i + j + k + l + "px" }), d.css({ "max-height": q - i - j - k - l - n.vert + "px", "overflow-y": "auto", "min-height": Math.max(e - n.vert, 0) + "px" })
                    };
                    z(), this.$searchbox.off("input.getSize propertychange.getSize").on("input.getSize propertychange.getSize", z), e.off("resize.getSize scroll.getSize").on("resize.getSize scroll.getSize", z)
                } else if (this.options.size && "auto" != this.options.size && this.$lis.not(p).length > this.options.size) {
                    var A = this.$lis.not(".divider").not(p).children().slice(0, this.options.size).last().parent().index(),
                        B = this.$lis.slice(0, A + 1).filter(".divider").length;
                    q = h * this.options.size + B * m + n.vert, b.options.container ? (c.data("height") || c.data("height", c.height()), s = c.data("height")) : s = c.height(), b.options.dropupAuto && this.$newElement.toggleClass("dropup", u > v && q - o.vert < s), c.css({ "max-height": q + i + j + k + l + "px", overflow: "hidden", "min-height": "" }), d.css({ "max-height": q - n.vert + "px", "overflow-y": "auto", "min-height": "" })
                }
            }
        },
        setWidth: function() {
            if ("auto" === this.options.width) {
                this.$menu.css("min-width", "0");
                var a = this.$menu.parent().clone().appendTo("body"),
                    b = this.options.container ? this.$newElement.clone().appendTo("body") : a,
                    c = a.children(".dropdown-menu").outerWidth(),
                    d = b.css("width", "auto").children("button").outerWidth();
                a.remove(), b.remove(), this.$newElement.css("width", Math.max(c, d) + "px")
            } else "fit" === this.options.width ? (this.$menu.css("min-width", ""), this.$newElement.css("width", "").addClass("fit-width")) : this.options.width ? (this.$menu.css("min-width", ""), this.$newElement.css("width", this.options.width)) : (this.$menu.css("min-width", ""), this.$newElement.css("width", ""));
            this.$newElement.hasClass("fit-width") && "fit" !== this.options.width && this.$newElement.removeClass("fit-width")
        },
        selectPosition: function() {
            this.$bsContainer = a('<div class="bs-container" />');
            var d, e, f, b = this,
                c = a(this.options.container),
                g = function(a) { b.$bsContainer.addClass(a.attr("class").replace(/form-control|fit-width/gi, "")).toggleClass("dropup", a.hasClass("dropup")), d = a.offset(), c.is("body") ? e = { top: 0, left: 0 } : (e = c.offset(), e.top += parseInt(c.css("borderTopWidth")) - c.scrollTop(), e.left += parseInt(c.css("borderLeftWidth")) - c.scrollLeft()), f = a.hasClass("dropup") ? 0 : a[0].offsetHeight, b.$bsContainer.css({ top: d.top - e.top + f, left: d.left - e.left, width: a[0].offsetWidth }) };
            this.$button.on("click", function() {
                var c = a(this);
                b.isDisabled() || (g(b.$newElement), b.$bsContainer.appendTo(b.options.container).toggleClass("open", !c.hasClass("open")).append(b.$menu))
            }), a(window).on("resize scroll", function() { g(b.$newElement) }), this.$element.on("hide.bs.select", function() { b.$menu.data("height", b.$menu.height()), b.$bsContainer.detach() })
        },
        setSelected: function(a, b, c) { c || (this.togglePlaceholder(), c = this.findLis().eq(this.liObj[a])), c.toggleClass("selected", b).find("a").attr("aria-selected", b) },
        setDisabled: function(a, b, c) { c || (c = this.findLis().eq(this.liObj[a])), b ? c.addClass("disabled").children("a").attr("href", "#").attr("tabindex", -1).attr("aria-disabled", !0) : c.removeClass("disabled").children("a").removeAttr("href").attr("tabindex", 0).attr("aria-disabled", !1) },
        isDisabled: function() { return this.$element[0].disabled },
        checkDisabled: function() {
            var a = this;
            this.isDisabled() ? (this.$newElement.addClass("disabled"), this.$button.addClass("disabled").attr("tabindex", -1).attr("aria-disabled", !0)) : (this.$button.hasClass("disabled") && (this.$newElement.removeClass("disabled"), this.$button.removeClass("disabled").attr("aria-disabled", !1)), this.$button.attr("tabindex") != -1 || this.$element.data("tabindex") || this.$button.removeAttr("tabindex")), this.$button.click(function() { return !a.isDisabled() })
        },
        togglePlaceholder: function() {
            var a = this.$element.val();
            this.$button.toggleClass("bs-placeholder", null === a || "" === a || a.constructor === Array && 0 === a.length)
        },
        tabIndex: function() { this.$element.data("tabindex") !== this.$element.attr("tabindex") && this.$element.attr("tabindex") !== -98 && "-98" !== this.$element.attr("tabindex") && (this.$element.data("tabindex", this.$element.attr("tabindex")), this.$button.attr("tabindex", this.$element.data("tabindex"))), this.$element.attr("tabindex", -98) },
        clickListener: function() {
            var b = this,
                d = a(document);
            this.$newElement.on("touchstart.dropdown", ".dropdown-menu", function(a) { a.stopPropagation() }), d.data("spaceSelect", !1), this.$button.on("keyup", function(a) { /(32)/.test(a.keyCode.toString(10)) && d.data("spaceSelect") && (a.preventDefault(), d.data("spaceSelect", !1)) }), this.$button.on("click", function() { b.setSize() }), this.$element.on("shown.bs.select", function() {
                if (b.options.liveSearch || b.multiple) {
                    if (!b.multiple) {
                        var a = b.liObj[b.$element[0].selectedIndex];
                        if ("number" != typeof a || b.options.size === !1) return;
                        var c = b.$lis.eq(a)[0].offsetTop - b.$menuInner[0].offsetTop;
                        c = c - b.$menuInner[0].offsetHeight / 2 + b.sizeInfo.liHeight / 2, b.$menuInner[0].scrollTop = c
                    }
                } else b.$menuInner.find(".selected a").focus()
            }), this.$menuInner.on("click", "li a", function(d) {
                var e = a(this),
                    f = e.parent().data("originalIndex"),
                    g = b.$element.val(),
                    h = b.$element.prop("selectedIndex"),
                    i = !0;
                if (b.multiple && 1 !== b.options.maxOptions && d.stopPropagation(), d.preventDefault(), !b.isDisabled() && !e.parent().hasClass("disabled")) {
                    var j = b.$element.find("option"),
                        k = j.eq(f),
                        l = k.prop("selected"),
                        m = k.parent("optgroup"),
                        n = b.options.maxOptions,
                        o = m.data("maxOptions") || !1;
                    if (b.multiple) {
                        if (k.prop("selected", !l), b.setSelected(f, !l), e.blur(), n !== !1 || o !== !1) {
                            var p = n < j.filter(":selected").length,
                                q = o < m.find("option:selected").length;
                            if (n && p || o && q)
                                if (n && 1 == n) j.prop("selected", !1), k.prop("selected", !0), b.$menuInner.find(".selected").removeClass("selected"), b.setSelected(f, !0);
                                else if (o && 1 == o) {
                                m.find("option:selected").prop("selected", !1), k.prop("selected", !0);
                                var r = e.parent().data("optgroup");
                                b.$menuInner.find('[data-optgroup="' + r + '"]').removeClass("selected"), b.setSelected(f, !0)
                            } else {
                                var s = "string" == typeof b.options.maxOptionsText ? [b.options.maxOptionsText, b.options.maxOptionsText] : b.options.maxOptionsText,
                                    t = "function" == typeof s ? s(n, o) : s,
                                    u = t[0].replace("{n}", n),
                                    v = t[1].replace("{n}", o),
                                    w = a('<div class="notify"></div>');
                                t[2] && (u = u.replace("{var}", t[2][n > 1 ? 0 : 1]), v = v.replace("{var}", t[2][o > 1 ? 0 : 1])), k.prop("selected", !1), b.$menu.append(w), n && p && (w.append(a("<div>" + u + "</div>")), i = !1, b.$element.trigger("maxReached.bs.select")), o && q && (w.append(a("<div>" + v + "</div>")), i = !1, b.$element.trigger("maxReachedGrp.bs.select")), setTimeout(function() { b.setSelected(f, !1) }, 10), w.delay(750).fadeOut(300, function() { a(this).remove() })
                            }
                        }
                    } else j.prop("selected", !1), k.prop("selected", !0), b.$menuInner.find(".selected").removeClass("selected").find("a").attr("aria-selected", !1), b.setSelected(f, !0);
                    !b.multiple || b.multiple && 1 === b.options.maxOptions ? b.$button.focus() : b.options.liveSearch && b.$searchbox.focus(), i && (g != b.$element.val() && b.multiple || h != b.$element.prop("selectedIndex") && !b.multiple) && (c = [f, k.prop("selected"), l], b.$element.triggerNative("change"))
                }
            }), this.$menu.on("click", "li.disabled a, .popover-title, .popover-title :not(.close)", function(c) { c.currentTarget == this && (c.preventDefault(), c.stopPropagation(), b.options.liveSearch && !a(c.target).hasClass("close") ? b.$searchbox.focus() : b.$button.focus()) }), this.$menuInner.on("click", ".divider, .dropdown-header", function(a) { a.preventDefault(), a.stopPropagation(), b.options.liveSearch ? b.$searchbox.focus() : b.$button.focus() }), this.$menu.on("click", ".popover-title .close", function() { b.$button.click() }), this.$searchbox.on("click", function(a) { a.stopPropagation() }), this.$menu.on("click", ".actions-btn", function(c) { b.options.liveSearch ? b.$searchbox.focus() : b.$button.focus(), c.preventDefault(), c.stopPropagation(), a(this).hasClass("bs-select-all") ? b.selectAll() : b.deselectAll() }), this.$element.change(function() { b.render(!1), b.$element.trigger("changed.bs.select", c), c = null })
        },
        liveSearchListener: function() {
            var b = this,
                c = a('<li class="no-results"></li>');
            this.$button.on("click.dropdown.data-api touchstart.dropdown.data-api", function() { b.$menuInner.find(".active").removeClass("active"), b.$searchbox.val() && (b.$searchbox.val(""), b.$lis.not(".is-hidden").removeClass("hidden"), c.parent().length && c.remove()), b.multiple || b.$menuInner.find(".selected").addClass("active"), setTimeout(function() { b.$searchbox.focus() }, 10) }), this.$searchbox.on("click.dropdown.data-api focus.dropdown.data-api touchend.dropdown.data-api", function(a) { a.stopPropagation() }), this.$searchbox.on("input propertychange", function() {
                if (b.$lis.not(".is-hidden").removeClass("hidden"), b.$lis.filter(".active").removeClass("active"), c.remove(), b.$searchbox.val()) {
                    var f, e = b.$lis.not(".is-hidden, .divider, .dropdown-header");
                    if (f = b.options.liveSearchNormalize ? e.not(":a" + b._searchStyle() + '("' + d(b.$searchbox.val()) + '")') : e.not(":" + b._searchStyle() + '("' + b.$searchbox.val() + '")'), f.length === e.length) c.html(b.options.noneResultsText.replace("{0}", '"' + h(b.$searchbox.val()) + '"')), b.$menuInner.append(c), b.$lis.addClass("hidden");
                    else {
                        f.addClass("hidden");
                        var i, g = b.$lis.not(".hidden");
                        g.each(function(b) {
                            var c = a(this);
                            c.hasClass("divider") ? void 0 === i ? c.addClass("hidden") : (i && i.addClass("hidden"), i = c) : c.hasClass("dropdown-header") && g.eq(b + 1).data("optgroup") !== c.data("optgroup") ? c.addClass("hidden") : i = null
                        }), i && i.addClass("hidden"), e.not(".hidden").first().addClass("active")
                    }
                }
            })
        },
        _searchStyle: function() { var a = { begins: "ibegins", startsWith: "ibegins" }; return a[this.options.liveSearchStyle] || "icontains" },
        val: function(a) { return "undefined" != typeof a ? (this.$element.val(a), this.render(), this.$element) : this.$element.val() },
        changeAll: function(b) {
            if (this.multiple) {
                "undefined" == typeof b && (b = !0), this.findLis();
                var c = this.$element.find("option"),
                    d = this.$lis.not(".divider, .dropdown-header, .disabled, .hidden"),
                    e = d.length,
                    f = [];
                if (b) { if (d.filter(".selected").length === d.length) return } else if (0 === d.filter(".selected").length) return;
                d.toggleClass("selected", b);
                for (var g = 0; g < e; g++) {
                    var h = d[g].getAttribute("data-original-index");
                    f[f.length] = c.eq(h)[0]
                }
                a(f).prop("selected", b), this.render(!1), this.togglePlaceholder(), this.$element.triggerNative("change")
            }
        },
        selectAll: function() { return this.changeAll(!0) },
        deselectAll: function() { return this.changeAll(!1) },
        toggle: function(a) { a = a || window.event, a && a.stopPropagation(), this.$button.trigger("click") },
        keydown: function(b) {
            var f, h, i, j, k, l, m, n, o, c = a(this),
                e = c.is("input") ? c.parent().parent() : c.parent(),
                g = e.data("this"),
                p = ":not(.disabled, .hidden, .dropdown-header, .divider)",
                q = { 32: " ", 48: "0", 49: "1", 50: "2", 51: "3", 52: "4", 53: "5", 54: "6", 55: "7", 56: "8", 57: "9", 59: ";", 65: "a", 66: "b", 67: "c", 68: "d", 69: "e", 70: "f", 71: "g", 72: "h", 73: "i", 74: "j", 75: "k", 76: "l", 77: "m", 78: "n", 79: "o", 80: "p", 81: "q", 82: "r", 83: "s", 84: "t", 85: "u", 86: "v", 87: "w", 88: "x", 89: "y", 90: "z", 96: "0", 97: "1", 98: "2", 99: "3", 100: "4", 101: "5", 102: "6", 103: "7", 104: "8", 105: "9" };
            if (g.options.liveSearch && (e = c.parent().parent()), g.options.container && (e = g.$menu), f = a('[role="listbox"] li', e), o = g.$newElement.hasClass("open"), !o && (b.keyCode >= 48 && b.keyCode <= 57 || b.keyCode >= 96 && b.keyCode <= 105 || b.keyCode >= 65 && b.keyCode <= 90)) return g.options.container ? g.$button.trigger("click") : (g.setSize(), g.$menu.parent().addClass("open"), o = !0), void g.$searchbox.focus();
            if (g.options.liveSearch && (/(^9$|27)/.test(b.keyCode.toString(10)) && o && (b.preventDefault(), b.stopPropagation(), g.$button.click().focus()), f = a('[role="listbox"] li' + p, e), c.val() || /(38|40)/.test(b.keyCode.toString(10)) || 0 === f.filter(".active").length && (f = g.$menuInner.find("li"), f = g.options.liveSearchNormalize ? f.filter(":a" + g._searchStyle() + "(" + d(q[b.keyCode]) + ")") : f.filter(":" + g._searchStyle() + "(" + q[b.keyCode] + ")"))), f.length) {
                if (/(38|40)/.test(b.keyCode.toString(10))) h = f.index(f.find("a").filter(":focus").parent()), j = f.filter(p).first().index(), k = f.filter(p).last().index(), i = f.eq(h).nextAll(p).eq(0).index(), l = f.eq(h).prevAll(p).eq(0).index(), m = f.eq(i).prevAll(p).eq(0).index(), g.options.liveSearch && (f.each(function(b) { a(this).hasClass("disabled") || a(this).data("index", b) }), h = f.index(f.filter(".active")), j = f.first().data("index"), k = f.last().data("index"), i = f.eq(h).nextAll().eq(0).data("index"), l = f.eq(h).prevAll().eq(0).data("index"), m = f.eq(i).prevAll().eq(0).data("index")), n = c.data("prevIndex"), 38 == b.keyCode ? (g.options.liveSearch && h--, h != m && h > l && (h = l), h < j && (h = j), h == n && (h = k)) : 40 == b.keyCode && (g.options.liveSearch && h++, h == -1 && (h = 0), h != m && h < i && (h = i), h > k && (h = k), h == n && (h = j)), c.data("prevIndex", h), g.options.liveSearch ? (b.preventDefault(), c.hasClass("dropdown-toggle") || (f.removeClass("active").eq(h).addClass("active").children("a").focus(), c.focus())) : f.eq(h).children("a").focus();
                else if (!c.is("input")) {
                    var s, t, r = [];
                    f.each(function() { a(this).hasClass("disabled") || a.trim(a(this).children("a").text().toLowerCase()).substring(0, 1) == q[b.keyCode] && r.push(a(this).index()) }), s = a(document).data("keycount"), s++, a(document).data("keycount", s), t = a.trim(a(":focus").text().toLowerCase()).substring(0, 1), t != q[b.keyCode] ? (s = 1, a(document).data("keycount", s)) : s >= r.length && (a(document).data("keycount", 0), s > r.length && (s = 1)), f.eq(r[s - 1]).children("a").focus()
                }
                if ((/(13|32)/.test(b.keyCode.toString(10)) || /(^9$)/.test(b.keyCode.toString(10)) && g.options.selectOnTab) && o) {
                    if (/(32)/.test(b.keyCode.toString(10)) || b.preventDefault(), g.options.liveSearch) /(32)/.test(b.keyCode.toString(10)) || (g.$menuInner.find(".active a").click(),
                        c.focus());
                    else {
                        var u = a(":focus");
                        u.click(), u.focus(), b.preventDefault(), a(document).data("spaceSelect", !0)
                    }
                    a(document).data("keycount", 0)
                }(/(^9$|27)/.test(b.keyCode.toString(10)) && o && (g.multiple || g.options.liveSearch) || /(27)/.test(b.keyCode.toString(10)) && !o) && (g.$menu.parent().removeClass("open"), g.options.container && g.$newElement.removeClass("open"), g.$button.focus())
            }
        },
        mobile: function() { this.$element.addClass("mobile-device") },
        refresh: function() { this.$lis = null, this.liObj = {}, this.reloadLi(), this.render(), this.checkDisabled(), this.liHeight(!0), this.setStyle(), this.setWidth(), this.$lis && this.$searchbox.trigger("propertychange"), this.$element.trigger("refreshed.bs.select") },
        hide: function() { this.$newElement.hide() },
        show: function() { this.$newElement.show() },
        remove: function() { this.$newElement.remove(), this.$element.remove() },
        destroy: function() { this.$newElement.before(this.$element).remove(), this.$bsContainer ? this.$bsContainer.remove() : this.$menu.remove(), this.$element.off(".bs.select").removeData("selectpicker").removeClass("bs-select-hidden selectpicker") }
    };
    var l = a.fn.selectpicker;
    a.fn.selectpicker = k, a.fn.selectpicker.Constructor = j, a.fn.selectpicker.noConflict = function() { return a.fn.selectpicker = l, this }, a(document).data("keycount", 0).on("keydown.bs.select", '.bootstrap-select [data-toggle=dropdown], .bootstrap-select [role="listbox"], .bs-searchbox input', j.prototype.keydown).on("focusin.modal", '.bootstrap-select [data-toggle=dropdown], .bootstrap-select [role="listbox"], .bs-searchbox input', function(a) { a.stopPropagation() }), a(window).on("load.bs.select.data-api", function() {
        a(".selectpicker").each(function() {
            var b = a(this);
            k.call(b, b.data())
        })
    })
}(jQuery);


/*
  jQuery Ketchup Plugin - Tasty Form Validation
  ---------------------------------------------
  
  Version 0.3.1 - 12. Jan 2011
  
  Copyright (c) 2011 by Sebastian Senf:
    https://mustardamus.com/
    https://usejquery.com/
    https://twitter.com/mustardamus

  Dual licensed under the MIT and GPL licenses:
    https://www.opensource.org/licenses/mit-license.php
    https://www.gnu.org/licenses/gpl.html

  Demo: https://demos.usejquery.com/ketchup-plugin/
  Repo: https://github.com/mustardamus/ketchup-plugin
  */

(function(g) {
    g.ketchup = {
        defaults: { attribute: "data-validate", validateIndicator: "validate", eventIndicator: "on", validateEvents: "blur", validateElements: ["input", "textarea", "select"], createErrorContainer: null, showErrorContainer: null, hideErrorContainer: null, addErrorMessages: null },
        dataNames: { validationString: "ketchup-validation-string", validations: "ketchup-validations", events: "ketchup-events", elements: "ketchup-validation-elements", container: "ketchup-container" },
        validations: {},
        helpers: {},
        validation: function(b,
            c, d, h) {
            var j;
            if (typeof c == "function") c = c;
            else {
                j = c;
                c = d
            }
            this.validations[b] = { message: j, func: c, init: h || function() {} };
            return this
        },
        message: function(b, c) { this.addMessage(b, c); return this },
        messages: function(b) { for (name in b) this.addMessage(name, b[name]); return this },
        addMessage: function(b, c) { if (this.validations[b]) this.validations[b].message = c },
        helper: function(b, c) { this.helpers[b] = c; return this },
        init: function(b, c, d) {
            this.options = c;
            var h = this;
            c = this.initFunctions().initFields(b, d);
            c.each(function() {
                var j =
                    g(this);
                h.bindValidationEvent(b, j).callInitFunctions(b, j)
            });
            b.data(this.dataNames.elements, c);
            this.bindFormSubmit(b)
        },
        initFunctions: function() {
            var b = this.options,
                c = ["createErrorContainer", "showErrorContainer", "hideErrorContainer", "addErrorMessages"];
            for (f = 0; f < c.length; f++) {
                var d = c[f];
                b[d] || (b[d] = this[d])
            }
            return this
        },
        initFields: function(b, c) {
            var d = this,
                h = this.dataNames,
                j = g(!c ? this.fieldsFromForm(b) : this.fieldsFromObject(b, c));
            j.each(function() {
                var l = g(this),
                    m = d.extractValidations(l.data(h.validationString),
                        d.options.validateIndicator);
                l.data(h.validations, m)
            });
            return j
        },
        callInitFunctions: function(b, c) { var d = c.data(this.dataNames.validations); for (i = 0; i < d.length; i++) d[i].init.apply(this.helpers, [b, c]) },
        fieldsFromForm: function(b) {
            var c = this,
                d = this.options,
                h = this.dataNames,
                j = d.validateElements,
                l = [];
            j = typeof j == "string" ? [j] : j;
            for (i = 0; i < j.length; i++) {
                var m = b.find(j[i] + "[" + d.attribute + "*=" + d.validateIndicator + "]");
                m.each(function() {
                    var k = g(this),
                        p = k.attr(d.attribute),
                        q = c.extractEvents(p, d.eventIndicator);
                    k.data(h.validationString, p).data(h.events, q ? q : d.validateEvents)
                });
                l.push(m.get())
            }
            return this.normalizeArray(l)
        },
        fieldsFromObject: function(b, c) {
            var d = this.options,
                h = this.dataNames,
                j = [];
            for (s in c) {
                var l, m;
                if (typeof c[s] == "string") {
                    l = c[s];
                    m = d.validateEvents
                } else {
                    l = c[s][0];
                    m = c[s][1]
                }
                var k = b.find(s);
                l = this.mergeValidationString(k, l);
                m = this.mergeEventsString(k, m);
                k.data(h.validationString, d.validateIndicator + "(" + l + ")").data(h.events, m);
                j.push(k.get())
            }
            return this.normalizeArray(j)
        },
        mergeEventsString: function(b,
            c) {
            var d = b.data(this.dataNames.events),
                h = "";
            if (d) {
                d = d.split(" ");
                for (i = 0; i < d.length; i++)
                    if (c.indexOf(d[i]) == -1) h += " " + d[i]
            }
            return g.trim(c + h)
        },
        mergeValidationString: function(b, c) {
            var d = this.options,
                h = b.data(this.dataNames.validationString),
                j = function(k) { var p = k.name; if (k.arguments.length) p = p + "(" + k.arguments.join(",") + ")"; return p },
                l = function(k, p) {
                    for (i = 0; i < k.length; i++)
                        if (k[i].name == p.name) return true
                };
            if (h) {
                var m = this.extractValidations(d.validateIndicator + "(" + c + ")", d.validateIndicator);
                d = this.extractValidations(h,
                    d.validateIndicator);
                c = "";
                for (o = 0; o < d.length; o++) c += j(d[o]) + ",";
                for (n = 0; n < m.length; n++) l(d, m[n]) || (c += j(m[n]) + ",")
            }
            return c
        },
        bindFormSubmit: function(b) {
            var c = this;
            b.submit(function() { return c.allFieldsValid(b, true) })
        },
        allFieldsValid: function(b, c) {
            var d = this,
                h = true;
            b.data(this.dataNames.elements).each(function() {
                var j = g(this);
                if (d.validateElement(j, b) != true) {
                    c == true && d.triggerValidationEvents(j);
                    h = false
                }
            });
            b.trigger("formIs" + (h ? "Valid" : "Invalid"), [b]);
            return h
        },
        bindValidationEvent: function(b, c) {
            var d =
                this,
                h = this.options,
                j = this.dataNames,
                l = c.data(j.events).split(" ");
            for (i = 0; i < l.length; i++) {
                c.bind("ketchup." + l[i], function() {
                    var m = d.validateElement(c, b),
                        k = c.data(j.container);
                    if (m != true) {
                        if (!k) {
                            k = h.createErrorContainer(b, c);
                            c.data(j.container, k)
                        }
                        h.addErrorMessages(b, c, k, m);
                        h.showErrorContainer(b, c, k)
                    } else k && h.hideErrorContainer(b, c, k)
                });
                this.bindValidationEventBridge(c, l[i])
            }
            return this
        },
        bindValidationEventBridge: function(b, c) { b.bind(c, function() { b.trigger("ketchup." + c) }) },
        validateElement: function(b,
            c) {
            var d = [],
                h = b.data(this.dataNames.validations),
                j = [c, b, b.val()];
            for (i = 0; i < h.length; i++) h[i].func.apply(this.helpers, j.concat(h[i].arguments)) || d.push(h[i].message);
            c.trigger("fieldIs" + (d.length ? "Invalid" : "Valid"), [c, b]);
            return d.length ? d : true
        },
        elementIsValid: function(b) { var c = this.dataNames; if (b.data(c.validations)) { c = b.parentsUntil("form").last().parent(); return this.validateElement(b, c) == true ? true : false } else if (b.data(c.elements)) return this.allFieldsValid(b); return null },
        triggerValidationEvents: function(b) {
            for (var c =
                    b.data(this.dataNames.events).split(" "), d = 0; d < c.length; d++) b.trigger("ketchup." + c[d])
        },
        extractValidations: function(b, c) {
            for (var d = b.substr(b.indexOf(c) + c.length + 1), h = "", j = [], l = 0, m = [], k = 0; k < d.length; k++) switch (d.charAt(k)) {
                case "(":
                    h += "(";
                    l++;
                    break;
                case ")":
                    if (l) {
                        h += ")";
                        l--
                    } else j.push(g.trim(h));
                    break;
                case ",":
                    if (l) h += ",";
                    else {
                        j.push(g.trim(h));
                        h = ""
                    }
                    break;
                default:
                    h += d.charAt(k)
            }
            for (v = 0; v < j.length; v++) {
                l = j[v].indexOf("(");
                d = j[v];
                h = [];
                if (l != -1) {
                    d = g.trim(j[v].substr(0, l));
                    h = g.map(j[v].substr(d.length).split(","),
                        function(p) { return g.trim(p.replace("(", "").replace(")", "")) })
                }
                if ((l = this.validations[d]) && l.message) {
                    k = l.message;
                    for (a = 1; a <= h.length; a++) k = k.replace("{arg" + a + "}", h[a - 1]);
                    m.push({ name: d, arguments: h, func: l.func, message: k, init: l.init })
                }
            }
            return m
        },
        extractEvents: function(b, c) {
            var d = false,
                h = b.indexOf(c + "(");
            if (h != -1) d = b.substr(h + c.length + 1).split(")")[0];
            return d
        },
        normalizeArray: function(b) {
            var c = [];
            for (i = 0; i < b.length; i++)
                for (e = 0; e < b[i].length; e++) b[i][e] && c.push(b[i][e]);
            return c
        },
        createErrorContainer: function(b,
            c) { if (typeof b == "function") { this.defaults.createErrorContainer = b; return this } else { var d = c.offset(); return g("<div/>", { html: "<ul></ul><span></span>", "class": "ketchup-error", css: { top: d.top, left: d.left + c.outerWidth() - 20 } }).appendTo("body") } },
        showErrorContainer: function(b, c, d) { if (typeof b == "function") { this.defaults.showErrorContainer = b; return this } else d.show().animate({ top: c.offset().top - d.height(), opacity: 1 }, "fast") },
        hideErrorContainer: function(b, c, d) {
            if (typeof b == "function") {
                this.defaults.hideErrorContainer =
                    b;
                return this
            } else d.animate({ top: c.offset().top, opacity: 0 }, "fast", function() { d.hide() })
        },
        addErrorMessages: function(b, c, d, h) {
            if (typeof b == "function") { this.defaults.addErrorMessages = b; return this } else {
                b = d.children("ul");
                b.html("");
                for (i = 0; i < h.length; i++) g("<li/>", { text: h[i] }).appendTo(b)
            }
        }
    };
    g.fn.ketchup = function(b, c) {
        var d = g(this);
        if (typeof b == "string") switch (b) {
            case "validate":
                g.ketchup.triggerValidationEvents(d);
                break;
            case "isValid":
                return g.ketchup.elementIsValid(d)
        } else this.each(function() {
            g.ketchup.init(d,
                g.extend({}, g.ketchup.defaults, b), c)
        });
        return this
    }
})(jQuery);
jQuery.ketchup.validation("required", "This field is required.", function(g, b, c) { g = b.attr("type").toLowerCase(); return g == "checkbox" || g == "radio" ? b.attr("checked") == true : c.length != 0 }).validation("minlength", "This field must have a minimal length of {arg1}.", function(g, b, c, d) { return c.length >= +d }).validation("maxlength", "This field must have a maximal length of {arg1}.", function(g, b, c, d) { return c.length <= +d }).validation("rangelength", "This field must have a length between {arg1} and {arg2}.", function(g,
    b, c, d, h) { return c.length >= d && c.length <= h }).validation("min", "Must be at least {arg1}.", function(g, b, c, d) { return this.isNumber(c) && +c >= +d }).validation("max", "Can not be greater than {arg1}.", function(g, b, c, d) { return this.isNumber(c) && +c <= +d }).validation("range", "Must be between {arg1} and {arg2}.", function(g, b, c, d, h) { return this.isNumber(c) && +c >= +d && +c <= +h }).validation("number", "Must be a number.", function(g, b, c) { return this.isNumber(c) }).validation("digits", "Must be digits.", function(g, b, c) { return /^\d+$/.test(c) }).validation("email",
    "Must be a valid E-Mail.",
    function(g, b, c) { return this.isEmail(c) }).validation("url", "Must be a valid URL.", function(g, b, c) { return this.isUrl(c) }).validation("username", "Must be a valid username.", function(g, b, c) { return this.isUsername(c) }).validation("match", "Must be {arg1}.", function(g, b, c, d) { return b.val() == d }).validation("contain", "Must contain {arg1}", function(g, b, c, d) { return this.contains(c, d) }).validation("date", "Must be a valid date.", function(g, b, c) { return this.isDate(c) }).validation("minselect",
    "Select at least {arg1} checkboxes.",
    function(g, b, c, d) { return d <= this.inputsWithName(g, b).filter(":checked").length },
    function(g, b) { this.bindBrothers(g, b) }).validation("maxselect", "Select not more than {arg1} checkboxes.", function(g, b, c, d) { return d >= this.inputsWithName(g, b).filter(":checked").length }, function(g, b) { this.bindBrothers(g, b) }).validation("rangeselect", "Select between {arg1} and {arg2} checkboxes.", function(g, b, c, d, h) {
    g = this.inputsWithName(g, b).filter(":checked").length;
    return d <= g && h >=
        g
}, function(g, b) { this.bindBrothers(g, b) });
jQuery.ketchup.helper("isNumber", function(g) { return /^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$/.test(g) }).helper("contains", function(g, b) { return g.indexOf(b) != -1 }).helper("isEmail", function(g) { return /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i.test(g) }).helper("isUrl", function(g) { return /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(g) }).helper("isUsername",
    function(g) { return /^([a-zA-Z])[a-zA-Z_-]*[\w_-]*[\S]$|^([a-zA-Z])[0-9_-]*[\S]$|^[a-zA-Z]*[\S]$/.test(g) }).helper("isDate", function(g) { return !/Invalid|NaN/.test(new Date(g)) }).helper("inputsWithName", function(g, b) { return $('input[name="' + b.attr("name") + '"]', g) }).helper("inputsWithNameNotSelf", function(g, b) { return this.inputsWithName(g, b).filter(function() { return $(this).index() != b.index() }) }).helper("getKetchupEvents", function(g) {
    g = g.data("events").ketchup;
    var b = [];
    for (i = 0; i < g.length; i++) b.push(g[i].namespace);
    return b.join(" ")
}).helper("bindBrothers", function(g, b) { this.inputsWithNameNotSelf(g, b).bind(this.getKetchupEvents(b), function() { b.ketchup("validate") }) });

/* 
 * Sidenav - off-canvas sidebar accordion menu
 */
! function(a) {
    "use strict";

    function b(b, d) { this.$el = a(b), this.opt = a.extend(!0, {}, c, d), this.init(this) }
    var c = {};
    b.prototype = {
        init: function(a) { a.initToggle(a), a.initDropdown(a) },
        initToggle: function(b) {
            a(document).on("click", function(c) {
                var d = a(c.target);
                d.closest(b.$el.data("sidenav-toggle"))[0] ? (b.$el.toggleClass("show"), a("body").toggleClass("sidenav-no-scroll"), b.toggleOverlay()) : d.closest(b.$el)[0] || (b.$el.removeClass("show"), a("body").removeClass("sidenav-no-scroll"), b.hideOverlay())
            })
        },
        initDropdown: function(b) {
            b.$el.on("click", "[data-sidenav-dropdown-toggle]", function(b) {
                var c = a(this);
                c.next("[data-sidenav-dropdown]").slideToggle("fast"), c.find("[data-sidenav-dropdown-icon]").toggleClass("show"), b.preventDefault()
            })
        },
        toggleOverlay: function() {
            var b = a("[data-sidenav-overlay]");
            b[0] || (b = a('<div data-sidenav-overlay class="sidenav-overlay"/>'), a("body").append(b)), b.fadeToggle("fast")
        },
        hideOverlay: function() { a("[data-sidenav-overlay]").fadeOut("fast") }
    }, a.fn.sidenav = function(c) { return this.each(function() { a.data(this, "sidenav") || a.data(this, "sidenav", new b(this, c)) }) }
}(window.jQuery);


/*!
 * headroom.js v0.9.3 - Give your page some headroom. Hide your header until you need it
 * Copyright (c) 2016 Nick Williams - https://wicky.nillia.ms/headroom.js
 * License: MIT
 */

! function(a, b) { "use strict"; "function" == typeof define && define.amd ? define([], b) : "object" == typeof exports ? module.exports = b() : a.Headroom = b() }(this, function() {
    "use strict";

    function a(a) { this.callback = a, this.ticking = !1 }

    function b(a) { return a && "undefined" != typeof window && (a === window || a.nodeType) }

    function c(a) { if (arguments.length <= 0) throw new Error("Missing arguments in extend function"); var d, e, f = a || {}; for (e = 1; e < arguments.length; e++) { var g = arguments[e] || {}; for (d in g) "object" != typeof f[d] || b(f[d]) ? f[d] = f[d] || g[d] : f[d] = c(f[d], g[d]) } return f }

    function d(a) { return a === Object(a) ? a : { down: a, up: a } }

    function e(a, b) { b = c(b, e.options), this.lastKnownScrollY = 0, this.elem = a, this.tolerance = d(b.tolerance), this.classes = b.classes, this.offset = b.offset, this.scroller = b.scroller, this.initialised = !1, this.onPin = b.onPin, this.onUnpin = b.onUnpin, this.onTop = b.onTop, this.onNotTop = b.onNotTop, this.onBottom = b.onBottom, this.onNotBottom = b.onNotBottom }
    var f = { bind: !! function() {}.bind, classList: "classList" in document.documentElement, rAF: !!(window.requestAnimationFrame || window.webkitRequestAnimationFrame || window.mozRequestAnimationFrame) };
    return window.requestAnimationFrame = window.requestAnimationFrame || window.webkitRequestAnimationFrame || window.mozRequestAnimationFrame, a.prototype = { constructor: a, update: function() { this.callback && this.callback(), this.ticking = !1 }, requestTick: function() { this.ticking || (requestAnimationFrame(this.rafCallback || (this.rafCallback = this.update.bind(this))), this.ticking = !0) }, handleEvent: function() { this.requestTick() } }, e.prototype = {
        constructor: e,
        init: function() { return e.cutsTheMustard ? (this.debouncer = new a(this.update.bind(this)), this.elem.classList.add(this.classes.initial), setTimeout(this.attachEvent.bind(this), 100), this) : void 0 },
        destroy: function() {
            var a = this.classes;
            this.initialised = !1, this.elem.classList.remove(a.unpinned, a.pinned, a.top, a.notTop, a.initial), this.scroller.removeEventListener("scroll", this.debouncer, !1)
        },
        attachEvent: function() { this.initialised || (this.lastKnownScrollY = this.getScrollY(), this.initialised = !0, this.scroller.addEventListener("scroll", this.debouncer, !1), this.debouncer.handleEvent()) },
        unpin: function() {
            var a = this.elem.classList,
                b = this.classes;
            !a.contains(b.pinned) && a.contains(b.unpinned) || (a.add(b.unpinned), a.remove(b.pinned), this.onUnpin && this.onUnpin.call(this))
        },
        pin: function() {
            var a = this.elem.classList,
                b = this.classes;
            a.contains(b.unpinned) && (a.remove(b.unpinned), a.add(b.pinned), this.onPin && this.onPin.call(this))
        },
        top: function() {
            var a = this.elem.classList,
                b = this.classes;
            a.contains(b.top) || (a.add(b.top), a.remove(b.notTop), this.onTop && this.onTop.call(this))
        },
        notTop: function() {
            var a = this.elem.classList,
                b = this.classes;
            a.contains(b.notTop) || (a.add(b.notTop), a.remove(b.top), this.onNotTop && this.onNotTop.call(this))
        },
        bottom: function() {
            var a = this.elem.classList,
                b = this.classes;
            a.contains(b.bottom) || (a.add(b.bottom), a.remove(b.notBottom), this.onBottom && this.onBottom.call(this))
        },
        notBottom: function() {
            var a = this.elem.classList,
                b = this.classes;
            a.contains(b.notBottom) || (a.add(b.notBottom), a.remove(b.bottom), this.onNotBottom && this.onNotBottom.call(this))
        },
        getScrollY: function() { return void 0 !== this.scroller.pageYOffset ? this.scroller.pageYOffset : void 0 !== this.scroller.scrollTop ? this.scroller.scrollTop : (document.documentElement || document.body.parentNode || document.body).scrollTop },
        getViewportHeight: function() { return window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight },
        getElementPhysicalHeight: function(a) { return Math.max(a.offsetHeight, a.clientHeight) },
        getScrollerPhysicalHeight: function() { return this.scroller === window || this.scroller === document.body ? this.getViewportHeight() : this.getElementPhysicalHeight(this.scroller) },
        getDocumentHeight: function() {
            var a = document.body,
                b = document.documentElement;
            return Math.max(a.scrollHeight, b.scrollHeight, a.offsetHeight, b.offsetHeight, a.clientHeight, b.clientHeight)
        },
        getElementHeight: function(a) { return Math.max(a.scrollHeight, a.offsetHeight, a.clientHeight) },
        getScrollerHeight: function() { return this.scroller === window || this.scroller === document.body ? this.getDocumentHeight() : this.getElementHeight(this.scroller) },
        isOutOfBounds: function(a) {
            var b = 0 > a,
                c = a + this.getScrollerPhysicalHeight() > this.getScrollerHeight();
            return b || c
        },
        toleranceExceeded: function(a, b) { return Math.abs(a - this.lastKnownScrollY) >= this.tolerance[b] },
        shouldUnpin: function(a, b) {
            var c = a > this.lastKnownScrollY,
                d = a >= this.offset;
            return c && d && b
        },
        shouldPin: function(a, b) {
            var c = a < this.lastKnownScrollY,
                d = a <= this.offset;
            return c && b || d
        },
        update: function() {
            var a = this.getScrollY(),
                b = a > this.lastKnownScrollY ? "down" : "up",
                c = this.toleranceExceeded(a, b);
            this.isOutOfBounds(a) || (a <= this.offset ? this.top() : this.notTop(), a + this.getViewportHeight() >= this.getScrollerHeight() ? this.bottom() : this.notBottom(), this.shouldUnpin(a, c) ? this.unpin() : this.shouldPin(a, c) && this.pin(), this.lastKnownScrollY = a)
        }
    }, e.options = { tolerance: { up: 0, down: 0 }, offset: 0, scroller: window, classes: { pinned: "headroom--pinned", unpinned: "headroom--unpinned", top: "headroom--top", notTop: "headroom--not-top", bottom: "headroom--bottom", notBottom: "headroom--not-bottom", initial: "headroom" } }, e.cutsTheMustard = "undefined" != typeof f && f.rAF && f.bind && f.classList, e
});

/*!
 * headroom.js v0.9.3 - Give your page some headroom. Hide your header until you need it
 * Copyright (c) 2016 Nick Williams - https://wicky.nillia.ms/headroom.js
 * License: MIT
 */

! function(a) {
    a && (a.fn.headroom = function(b) {
        return this.each(function() {
            var c = a(this),
                d = c.data("headroom"),
                e = "object" == typeof b && b;
            e = a.extend(!0, {}, Headroom.options, e), d || (d = new Headroom(this, e), d.init(), c.data("headroom", d)), "string" == typeof b && (d[b](), "destroy" === b && c.removeData("headroom"))
        })
    }, a("[data-headroom]").each(function() {
        var b = a(this);
        b.headroom(b.data())
    }))
}(window.Zepto || window.jQuery);


/*global jQuery */
/*!
 * FitText.js 1.2
 *
 * Copyright 2011, Dave Rupert https://daverupert.com
 * Released under the WTFPL license
 * https://sam.zoy.org/wtfpl/
 *
 * Date: Thu May 05 14:23:00 2011 -0600
 */

(function($) {

    $.fn.fitText = function(kompressor, options) {

        // Setup options
        var compressor = kompressor || 1,
            settings = $.extend({
                'minFontSize': Number.NEGATIVE_INFINITY,
                'maxFontSize': Number.POSITIVE_INFINITY
            }, options);

        return this.each(function() {

            // Store the object
            var $this = $(this);

            // Resizer() resizes items based on the object width divided by the compressor * 10
            var resizer = function() {
                $this.css('font-size', Math.max(Math.min($this.width() / (compressor * 10), parseFloat(settings.maxFontSize)), parseFloat(settings.minFontSize)));
            };

            // Call once to set.
            resizer();

            // Call on resize. Opera debounces their resize by default.
            $(window).on('resize.fittext orientationchange.fittext', resizer);

        });

    };

})(jQuery);