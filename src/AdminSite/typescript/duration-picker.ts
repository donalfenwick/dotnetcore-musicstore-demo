/**
 * JQuery plugin for selecting a duration of time for setting album track length
 * adapted from the following JSFiddle https://jsfiddle.net/0odpuwv9/51/
 */

// register the plugin class as a jquery plugin
(function ($: JQueryStatic) {
    $.fn.durationPicker = function (options: IDurationPickerOptions) {
       return this.each(function () {
          new DurationPicker($(this), options);
       });
    };
 })(jQuery);
 
interface IDurationPickerOptions{
    hours?: DurationPickerInput,
    minutes?: DurationPickerInput,
    seconds?: DurationPickerInput,
    classname: string,
    type: string,
    responsive: boolean,
    [key:string]: any
}
interface DurationPickerInput{
    label: 'h'|'m'|'s';
    min: number;
    max: number;
}
interface DurationValues{
    [key:string]: any;
    hours: number;
    minutes: number;
    seconds: number;
}
class DurationPicker{

    private settings: IDurationPickerOptions;
    private stages: string[];
    private template: string;
    private jqitem: JQuery<HTMLElement>;
    private jqchildren: JQuery<HTMLElement>;
    private element: JQuery<HTMLElement>;

    constructor (element: JQuery<HTMLElement>, options: IDurationPickerOptions) {

        var defaults: IDurationPickerOptions = {
            classname: 'form-control',
            type: 'number',
            responsive: true
        };

        this.settings = $.extend(true, {}, defaults, options);;
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

    setup() {
        this.element.before(this.jqitem);
        this.element.hide();

        // setup the editor based on initial input
        var sec_num = 0;
        if(this.element){ 
            let initValue = this.element.val();
            if(initValue){
                sec_num = parseInt(initValue.toString());
            }
        }
        if(!isNaN(sec_num)){
            let hours   = Math.floor(sec_num / 3600) % 24;
            let minutes = Math.floor(sec_num / 60) % 60;
            let seconds = sec_num % 60;
            
            $(this.element).prev().find('.durationpicker-duration-hours').val(hours);
            $(this.element).prev().find('.durationpicker-duration-minutes').val(minutes);
            $(this.element).prev().find('.durationpicker-duration-seconds').val(seconds);
        }


        this.jqchildren.find(".durationpicker-duration").on('change', {ths: this}, function (ev) {
            let element = ev.data.ths.element;


            let value = "";
            let total_seconds = 0;
            $(this).parent().parent().find('input').each(function () {
                let input = $(this);
                if(input.hasClass('durationpicker-duration-seconds')){
                    let rawVal = input.val();
                    if(rawVal){
                        let parsed = parseInt(rawVal.toString());
                        if(!isNaN(parsed)){
                            total_seconds += parsed;
                        }
                    }
                }else if(input.hasClass('durationpicker-duration-minutes')){
                    let rawVal = input.val();
                    if(rawVal){
                        let parsed = parseInt(rawVal.toString());
                        if(!isNaN(parsed)){
                            total_seconds += parsed * 60;
                        }
                    }
                }else if(input.hasClass('durationpicker-duration-hours')){
                    let rawVal = input.val();
                    if(rawVal){
                        let parsed = parseInt(rawVal.toString());
                        if(!isNaN(parsed)){
                            total_seconds += (parsed * 60) * 60;
                        }
                    }
                }
                let val = 0;
                if (input.val() != null && input.val() != ""){
                    let rawVal = input.val();
                    if(rawVal){
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

    }
    resize() {
        if (!this.settings.responsive) {
            return;
        }
        var padding = parseInt(this.jqitem.css('padding-left').split('px')[0]) + parseInt(this.jqitem.css('padding-right').split('px')[0]);
        var minwidth = padding;
        var minheight = padding;
        this.jqchildren.each(function () {
            let ths = $(this);
            if(ths !== undefined){
                minwidth = minwidth + (ths.outerWidth() ? <number>ths.outerWidth() : 0);
                minheight = minheight + (ths.outerHeight() ? <number>ths.outerHeight() : 0);
            }
        });
        if (this.jqitem && ((this.jqitem.parent().width()) ? <number>this.jqitem.parent().width() : 0) < minwidth) {
            this.jqchildren.each(function () {
                let ths = $(this);
                ths.css('display', 'block');
            });
            this.jqitem.css('height', minheight)
        }
        else {
            this.jqchildren.each(function () {
                let ths = $(this);
                ths.css('display', 'inline-block');
            });
        }
    }
    private getitem() {
        return this.jqitem;
    }
    private setvalues(values: DurationValues) {
        this.set_values(values, this);
        $(".durationpicker-duration").trigger('change');
    }
    private disable() {
        this.jqchildren.children("input").each(function (index, item) {
            (<HTMLInputElement>item).readOnly = true;
        });
    }
    private enable () {
        this.jqchildren.children("input").each(function (index, item) {
            (<HTMLInputElement>item).readOnly = false;
        });
    }

    private set_values(values:DurationValues, self: DurationPicker){
        for (var value in Object.keys(values)){
            if (self.stages.indexOf(Object.keys(values)[value]) != -1){
                let rawValue = values[(Object.keys(values)[value])];
                self.jqitem.find("#duration-" + (Object.keys(values)[value])).val(rawValue);
            }
        }
    }

    private get_stages(settings: any){
        var stages = [];
        for (let key in Object.keys(settings)){
            if (['classname', 'responsive', 'type'].indexOf(Object.keys(settings)[key]) == -1) {
                stages.push(Object.keys(settings)[key]);
            }
        }
        return stages;
    }

    private generate_template(settings: IDurationPickerOptions, stages: string[]) {
        var html = `<div class="durationpicker-container ${settings.classname}">`;
        var type = settings.type;
        for (let item in stages){
            html += '<div class="durationpicker-innercontainer">' +
            `<input min="${settings[stages[item]]['min']}" max="${settings[stages[item]]['max']}" placeholder="0" type="${type}" id="duration-${stages[item]}" class="durationpicker-duration durationpicker-duration-${stages[item]}" >`+
            `<span class="durationpicker-label">${settings[stages[item]]['label']}</span></div>`;
        }
        html += '</div>';

        return html;
    }

}
