// add type definition for the sortable plugin
declare function sortable(a: HTMLElement, options?:any): Element[];

(function($: JQueryStatic) {

    $.fn.reorderPostbackList = function() {

        return this.each(function() {
            
            var $editorInstance = $(this);
            
            // rebuild the field names and item index values after a resort so that the list items are
            // postback to the server side controller in the expected order
            function updateItemIndexes() {
                $editorInstance.find('.reorder-row').each(function (index: number) {
                    let $rowElement = $(this);
                    $rowElement.find('.item-index').val(index);

                    $rowElement.find('.postback-input').each(function(){
                        let nameAttrValue = $(this).attr('name');
                        if(nameAttrValue){
                            nameAttrValue=nameAttrValue.replace(/ *\[[^\]]*]/, '['+index+']');
                            $(this).attr('name', nameAttrValue);
                        }
                    });
                });
            }
            
            var sortableElement = sortable($editorInstance.get(0),{
                items: '.reorder-row',
                handle: '.sort-button'
            });
            sortableElement[0].addEventListener('sortupdate', function(event:any) {

                // update track index values
                updateItemIndexes();
            });
            sortableElement[0].addEventListener('sortstop', function(event: any) {

                // update track index values
                updateItemIndexes();
            });

        });
    };

}(jQuery));