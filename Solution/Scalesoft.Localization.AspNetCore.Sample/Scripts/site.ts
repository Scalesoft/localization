import {Localization, LocalizationErrorResolution} from "../../Scalesoft.Localization.Web.Script/src/localization";

namespace LocalizationSample {
    $(() => {
        new LocalizationTest().init();
    });

    class LocalizationTest {
        private readonly useAsync: boolean = true;
        private readonly localization: Localization;

        constructor() {
            this.localization = new Localization({
                errorResolution: LocalizationErrorResolution.Key,
                siteUrl: "/",
            });
        }

        public init() {
            this.testPluralization();
        }

        private testPluralization() {
            if (this.useAsync) {
                this.localization.getDictionaryAsync()
                    .then((dictionaryResult) => {
                        const translation = dictionaryResult.result.translate(
                            "Pluralization",
                            () => dictionaryResult.result.getFallbackTranslation(
                                "Pluralization",
                                "global",
                                undefined,
                            ),
                        );

                        $(".output").append(`<h3 title="Localized directly using dictionary">${translation.value}</h3>`);
                    });

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
                        $(".output").append(`<div>key="${key}" scope="${scope}" number=${value}: <strong>${value} ${translation.result.value}</strong></div>`);
                    });
            } else {
                const localizedString = this.localization.translatePluralization(key, value, scope);
                $(".output").append(`<div>key="${key}" scope="${scope}" number=${value}: <strong>${value} ${localizedString.value}</strong></div>`);
            }
        }
    }
}
