$(document).ready(() => {
    const editor = new LocalizationEditor();
    editor.init();
});

class LocalizationEditor {
    private baseUrl: string;
    private scopeListJq: JQuery<HTMLElement>;
    private selectedScopeJq: JQuery<HTMLElement>;
    private editorContainerJq: JQuery<HTMLElement>;

    constructor() {
        this.baseUrl = $("#js-data").attr("data-base-url");
        this.scopeListJq = $("#scope-list");
        this.selectedScopeJq = $("#selected-scope");
        this.editorContainerJq = $("#editor-container");
    }

    public init() {
        const hyperLinksJq = this.scopeListJq.find("a");
        hyperLinksJq.click((e) => {
            hyperLinksJq.removeClass("active");

            const hyperLinkJq = $(e.currentTarget);
            hyperLinkJq.addClass("active");

            const selectedScope = hyperLinkJq.text();
            this.selectedScopeJq.text(selectedScope);

            this.loadEditor(selectedScope);
        });
    }

    private loadEditor(scope: string) {
        this.editorContainerJq.load(this.baseUrl + "Home/GetEditor?scope=" + encodeURIComponent(scope),
            (responseText, responseStatus) => {
                if (responseStatus !== "success") {
                    this.editorContainerJq.text("Error loading dictionaries for selected scope");
                }
            });
    }
}