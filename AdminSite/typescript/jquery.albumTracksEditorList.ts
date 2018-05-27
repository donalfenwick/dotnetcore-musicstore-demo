


// add type definition for the sortable plugin
declare function sortable(a: HTMLElement, options?:any): Element[];

(function($) {
    var durationPickerOptions :IDurationPickerOptions = {
        minutes: { label: "m", min: 0, max: 59 },
        seconds: { label: "s", min: 0, max: 59},
        classname: 'form-control',
        type: 'number',
        responsive: true
    };
    $.fn.albumTracksEditorList = function() {

        return this.each( function() {
            
            var $editorInstance = $(this);
            $editorInstance.find('.item-duration').durationPicker(durationPickerOptions);
            var templateElement = document.getElementById("album-track-item-editor-template");
            var templateSource: string = ''; 
            if(templateElement){
                templateSource = templateElement.innerHTML;
            }
            var template = Handlebars.compile(templateSource);

            function addNewTrackItem(){
                let existingTracksCount = $editorInstance.find('.track-row').length;
                $(template({index: existingTracksCount, trackNo: existingTracksCount+1})).appendTo($editorInstance.find('.tracks-container'))
                        .find('.item-duration').durationPicker(durationPickerOptions);
                sortable($editorInstance.find('.tracks-container').get(0), '');
            }
            $editorInstance.on('click','.remove-button',function(){
                // find parent row...
                $(this).closest('.track-row').remove();
                updateItemIndexes();
            });

            $editorInstance.on('click','.add-track-btn',function(){
                addNewTrackItem();
            });
    
            function updateItemIndexes() {
                
                $editorInstance.find('.track-row').each(function (index) {
                    let $rowElement = $(this);
                    $rowElement.find('.track-no').html((index+1).toString());
                    $rowElement.find('.track-number-hidden').val(index);

                    $rowElement.find('.postback-input').each(function(){
                        let nameAttrValue = $(this).attr('name');
                        if(nameAttrValue){
                            // reg ex replace the index in Tracks[n].PropertyName
                            nameAttrValue=nameAttrValue.replace(/ *\[[^\]]*]/, '['+index+']');
                            $(this).attr('name', nameAttrValue);
                        }
                    });
                    $rowElement.find('.field-validation-valid').each(function(){
                        
                        let nameAttrValue = $(this).attr('data-valmsg-for');
                        if(nameAttrValue){
                            // reg ex replace the index in Tracks[n].PropertyName
                            nameAttrValue=nameAttrValue.replace(/ *\[[^\]]*]/, '['+index+']');
                            $(this).attr('data-valmsg-for', nameAttrValue);
                        }
                    });
                });
            }
            
            var sortableElement:Element[] = sortable($editorInstance.find('.tracks-container').get(0),{
                items: '.track-row',
                handle: '.sort-button'
            });
            sortableElement[0].addEventListener('sortupdate', function(e) {

                // update track index values
                updateItemIndexes();
            });
            sortableElement[0].addEventListener('sortstop', function(e) {

                // update track index values
                updateItemIndexes();
            });

        });
    };

}(jQuery));