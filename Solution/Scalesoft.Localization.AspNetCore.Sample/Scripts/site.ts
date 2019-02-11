namespace LocalizationSample {
    $(document.documentElement).ready(() => {
        new LocalizationTest().init();
    });

    class LocalizationTest {
        private localization: Localization;

        constructor() {
            this.localization = new Localization();
            this.localization.configureSiteUrl("/");
        }

        public init() {
            this.addPluralizationTestLine("years", -5);
            this.addPluralizationTestLine("years", -1);
            this.addPluralizationTestLine("years", 0);
            this.addPluralizationTestLine("years", 1);
            this.addPluralizationTestLine("years", 11);
        }

        private addPluralizationTestLine(key: string, value: number, scope?: string) {
            const localizedString = this.localization.translatePluralization(key, value, scope);
            $(".output").append(`<div>key="${key}" number=${value} scope="${scope}": ${localizedString.value}</div>`);
        }
    }
}
