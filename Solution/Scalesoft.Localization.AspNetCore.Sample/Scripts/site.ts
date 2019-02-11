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
            const localizedString = this.localization.translatePluralization("months", 3, "client");

            $(".output").append(`<div>key="months" number=3 scope="client": ${localizedString.value}</div>`);
        }
    }
}
