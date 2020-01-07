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
                } else {
                    this.initEditorEvents();
                }
            });
    }

    private initEditorEvents() {
        $(".edit-cell").click((e) => {
            const cellJq = $(e.currentTarget).closest("td");
            const value = cellJq.data("value");
            const culture = cellJq.data("culture");
            const key = $(e.currentTarget).closest("tr").data("key");

            if (cellJq.hasClass("modified-value")) {
                return;
            }

            const editorInputJq = $(`<div class="input-group">
    <input type="text" class="form-control translation-text">
    <div class="input-group-append">
        <button class="btn btn-outline-secondary cancel-button" type="button">Cancel</button>
    </div>
    <div class="backup d-none"></div>
</div>`);
            const originalJq = cellJq.children();
            const backupJq = editorInputJq.find(".backup");
            backupJq.append(originalJq);
            cellJq.append(editorInputJq);
            cellJq.addClass("modified-value");

            editorInputJq.find(".translation-text")
                .val(value)
                .focus();

            editorInputJq.find(".cancel-button").click((e) => {
                e.stopPropagation();
                cellJq.append(originalJq);
                cellJq.removeClass("modified-value");
                editorInputJq.remove();
            });
        });
    }
}