"use strict";
/**
 * JQuery plugin for selecting a duration of time for setting album track length
 * adapted from the following JSFiddle https://jsfiddle.net/0odpuwv9/51/
 */
// register the plugin class as a jquery plugin
(function ($) {
    $.fn.durationPicker = function (options) {
        return this.each(function () {
            new DurationPicker($(this), options);
        });
    };
})(jQuery);
var DurationPicker = /** @class */ (function () {
    function DurationPicker(element, options) {
        var defaults = {
            classname: 'form-control',
            type: 'number',
            responsive: true
        };
        this.settings = $.extend(true, {}, defaults, options);
        ;
        this.stages = this.get_stages(this.settings);
        this.template = this.generate_template(this.settings, this.stages);
        this.jqitem = $(this.template);
        this.jqchildren = this.jqitem.children();
        this.element = $(element);
        this.setup();
        this.resize();
        this.jqchildren.find(".durationpicker-duration").trigger('change');
        var _self = this;
    }
    DurationPicker.prototype.setup = function () {
        this.element.before(this.jqitem);
        this.element.hide();
        // setup the editor based on initial input
        var sec_num = 0;
        if (this.element) {
            var initValue = this.element.val();
            if (initValue) {
                sec_num = parseInt(initValue.toString());
            }
        }
        if (!isNaN(sec_num)) {
            var hours = Math.floor(sec_num / 3600) % 24;
            var minutes = Math.floor(sec_num / 60) % 60;
            var seconds = sec_num % 60;
            $(this.element).prev().find('.durationpicker-duration-hours').val(hours);
            $(this.element).prev().find('.durationpicker-duration-minutes').val(minutes);
            $(this.element).prev().find('.durationpicker-duration-seconds').val(seconds);
        }
        this.jqchildren.find(".durationpicker-duration").on('change', { ths: this }, function (ev) {
            var element = ev.data.ths.element;
            var value = "";
            var total_seconds = 0;
            $(this).parent().parent().find('input').each(function () {
                var input = $(this);
                if (input.hasClass('durationpicker-duration-seconds')) {
                    var rawVal = input.val();
                    if (rawVal) {
                        var parsed = parseInt(rawVal.toString());
                        if (!isNaN(parsed)) {
                            total_seconds += parsed;
                        }
                    }
                }
                else if (input.hasClass('durationpicker-duration-minutes')) {
                    var rawVal = input.val();
                    if (rawVal) {
                        var parsed = parseInt(rawVal.toString());
                        if (!isNaN(parsed)) {
                            total_seconds += parsed * 60;
                        }
                    }
                }
                else if (input.hasClass('durationpicker-duration-hours')) {
                    var rawVal = input.val();
                    if (rawVal) {
                        var parsed = parseInt(rawVal.toString());
                        if (!isNaN(parsed)) {
                            total_seconds += (parsed * 60) * 60;
                        }
                    }
                }
                var val = 0;
                if (input.val() != null && input.val() != "") {
                    var rawVal = input.val();
                    if (rawVal) {
                        val = parseInt(rawVal.toString());
                    }
                }
                value += val + input.next().text() + ",";
            });
            value = value.slice(0, -1);
            element.val(total_seconds);
        });
        $(".durationpicker-duration").trigger('change');
        window.addEventListener('resize', this.resize.bind(this));
    };
    DurationPicker.prototype.resize = function () {
        if (!this.settings.responsive) {
            return;
        }
        var padding = parseInt(this.jqitem.css('padding-left').split('px')[0]) + parseInt(this.jqitem.css('padding-right').split('px')[0]);
        var minwidth = padding;
        var minheight = padding;
        this.jqchildren.each(function () {
            var ths = $(this);
            if (ths !== undefined) {
                minwidth = minwidth + (ths.outerWidth() ? ths.outerWidth() : 0);
                minheight = minheight + (ths.outerHeight() ? ths.outerHeight() : 0);
            }
        });
        if (this.jqitem && ((this.jqitem.parent().width()) ? this.jqitem.parent().width() : 0) < minwidth) {
            this.jqchildren.each(function () {
                var ths = $(this);
                ths.css('display', 'block');
            });
            this.jqitem.css('height', minheight);
        }
        else {
            this.jqchildren.each(function () {
                var ths = $(this);
                ths.css('display', 'inline-block');
            });
        }
    };
    DurationPicker.prototype.getitem = function () {
        return this.jqitem;
    };
    DurationPicker.prototype.setvalues = function (values) {
        this.set_values(values, this);
        $(".durationpicker-duration").trigger('change');
    };
    DurationPicker.prototype.disable = function () {
        this.jqchildren.children("input").each(function (index, item) {
            item.readOnly = true;
        });
    };
    DurationPicker.prototype.enable = function () {
        this.jqchildren.children("input").each(function (index, item) {
            item.readOnly = false;
        });
    };
    DurationPicker.prototype.set_values = function (values, self) {
        for (var value in Object.keys(values)) {
            if (self.stages.indexOf(Object.keys(values)[value]) != -1) {
                var rawValue = values[(Object.keys(values)[value])];
                self.jqitem.find("#duration-" + (Object.keys(values)[value])).val(rawValue);
            }
        }
    };
    DurationPicker.prototype.get_stages = function (settings) {
        var stages = [];
        for (var key in Object.keys(settings)) {
            if (['classname', 'responsive', 'type'].indexOf(Object.keys(settings)[key]) == -1) {
                stages.push(Object.keys(settings)[key]);
            }
        }
        return stages;
    };
    DurationPicker.prototype.generate_template = function (settings, stages) {
        var html = "<div class=\"durationpicker-container " + settings.classname + "\">";
        var type = settings.type;
        for (var item in stages) {
            html += '<div class="durationpicker-innercontainer">' +
                ("<input min=\"" + settings[stages[item]]['min'] + "\" max=\"" + settings[stages[item]]['max'] + "\" placeholder=\"0\" type=\"" + type + "\" id=\"duration-" + stages[item] + "\" class=\"durationpicker-duration durationpicker-duration-" + stages[item] + "\" >") +
                ("<span class=\"durationpicker-label\">" + settings[stages[item]]['label'] + "</span></div>");
        }
        html += '</div>';
        return html;
    };
    return DurationPicker;
}());
