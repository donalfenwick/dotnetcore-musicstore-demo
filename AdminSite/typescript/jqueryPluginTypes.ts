//extend JQuery interface to include any custom plugins that need to be registered
interface JQuery{
    reorderPostbackList(): JQuery<HTMLElement>;
    albumTracksEditorList(): JQuery<HTMLElement>;
    durationPicker(options: IDurationPickerOptions): JQuery<HTMLElement>;
}