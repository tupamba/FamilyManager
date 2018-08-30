
// import translations
import {LANG_EN_NAME, LANG_EN_TRANS} from './lang-en';
import { LANG_ES_NAME, LANG_ES_TRANS } from './lang-es';


// translation token
export const TRANSLATIONS:  any = {};

// all translations
export const dictionary = {
    [LANG_EN_NAME]: LANG_EN_TRANS,
    [LANG_ES_NAME]: LANG_ES_TRANS
};

// providers
export const TRANSLATION_PROVIDERS = [
    { provide: TRANSLATIONS, useValue: dictionary },
];