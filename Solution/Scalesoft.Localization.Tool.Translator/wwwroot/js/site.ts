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
        this.updateUnsavedChangesAlert();

        $(".edit-cell").click((e) => {
            const cellJq = $(e.currentTarget).closest("td");
            const value = cellJq.data("value");
            
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

            this.updateUnsavedChangesAlert();

            editorInputJq.find(".translation-text")
                .val(value)
                .focus();

            editorInputJq.find(".cancel-button").click((e) => {
                e.stopPropagation();
                cellJq.append(originalJq);
                cellJq.removeClass("modified-value");
                editorInputJq.remove();
                this.updateUnsavedChangesAlert();
            });
        });

        $(".unsaved-changes-alert .saving-spinner").hide();

        $(".unsaved-changes-alert .save-button").click(() => {
            this.saveChanges();
        });
    }

    private updateUnsavedChangesAlert() {
        const alertJq = $(".unsaved-changes-alert");
        if ($(".modified-value").length > 0) {
            alertJq.show();
        } else {
            alertJq.hide();
        }
    }

    private getChangedItems(container: JQuery): ITranslationItemChange[] {
        const resultList: ITranslationItemChange[] = [];
        container.find(".modified-value").each((index, element) => {
            const cellJq = $(element);
            const culture = cellJq.data("culture") as string;
            const key = cellJq.closest("tr").data("key") as string;
            const value = cellJq.find(".translation-text").val() as string;

            resultList.push({
                culture: culture,
                key: key,
                value: value,
            });
        });

        return resultList;
    }

    private saveChanges() {
        const scope = $("#common-data").data("scope") as string;
        const standardDictionary = this.getChangedItems($(".standard-dictionary"));
        const constants = this.getChangedItems($(".constants"));
        const data: ISaveLocalization = {
            scope: scope,
            dictionary: standardDictionary,
            constants: constants
        }

        const savingSpinnerJq = $(".unsaved-changes-alert .saving-spinner");
        savingSpinnerJq.show();

        $.ajax({
            type: "POST",
            url: this.baseUrl + "Home/SaveChanges",
            data: JSON.stringify(data),
            contentType: "application/json"
        }).done(() => {
            $("#scope-list .active").click();
        }).fail(() => {
            alert("Saving failed");
        }).always(() => {
            savingSpinnerJq.hide();
        });
    }
}

interface ITranslationItemChange {
    culture: string;
    key: string;
    value: string;
}

interface ISaveLocalization {
    scope: string;
    dictionary: ITranslationItemChange[];
    constants: ITranslationItemChange[];
}