/// <reference path="../../Scalesoft.Localization.Web.Script/src/localization.ts" />
namespace LocalizationSample {
    $(document.documentElement).ready(() => {
        new LocalizationTest().init();
    });

    class LocalizationTest {
        private readonly useAsync: boolean = true;
        private readonly localization: Localization;

        constructor() {
            this.localization = new Localization();
            this.localization.configureSiteUrl("/");
        }

        public init() {
            this.testPluralization();
        }

        private testPluralization() {
            if (this.useAsync) {
                this.localization.translateAsync("Pluralization")
                    .then((translation) => {
                        $(".output").append(`<h2>${translation.result.value}</h2>`);
                        for (let i = -11; i < 12; i++) {
                            this.addPluralizationTestLine(i, "years");
                        }
                        for (let i = -11; i < 12; i++) {
                            this.addPluralizationTestLine(i, "cats", "non-global-pluralization");
                        }
                    });
            } else {
                const translatedString = this.localization.translate("Pluralization").value;
                $(".output").append(`<h2>${translatedString}</h2>`);
                for (let i = -11; i < 12; i++) {
                    this.addPluralizationTestLine(i, "years");
                }
                for (let i = -11; i < 12; i++) {
                    this.addPluralizationTestLine(i, "cats", "non-global-pluralization");
                }
            }
        }

        private addPluralizationTestLine(value: number, key: string, scope: string = null) {
            if (this.useAsync) {
                this.localization.translatePluralizationAsync(key, value, scope)
                    .then((translation) => {
                        $(".output").append(
                            `<div>key="${key}" scope="${scope}" number=${value}: <strong>${value} ${translation.result.value}</strong></div>`
                        );
                    });
            } else {
                const localizedString = this.localization.translatePluralization(key, value, scope);
                $(".output").append(`<div>key="${key}" scope="${scope}" number=${value}: <strong>${value} ${localizedString.value}</strong></div>`);
            }
        }
    }
}
