interface ILocalization {
    translate(text: string, scope?: string, cultureName?: string): string;
    translateFormat(text: string, parameters: string[], scope?: string, cultureName?: string): string;
}