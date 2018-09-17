import { Injectable } from '@angular/core';
import { dictionary } from '../../shared/lang/translation';

@Injectable()
export class TranslateService {
  private _currentLang: string = "es";

  public get currentLang() {
    return this._currentLang;
  }

  // inject our translations
  constructor() {
  }

  public use(lang: string): void {
    // set current language
    this._currentLang = lang;
  }
  public changeLang(): void {
    // set current language
    this._currentLang = this._currentLang == 'es' ? 'en' : 'es';
  }
  private translate(key: string): string {
    // private perform translation
    let translation = key;

    if (dictionary[this.currentLang] && dictionary[this.currentLang][key]) {
      return dictionary[this.currentLang][key];
    }

    return translation;
  }

  public instant(key: string) {
    // call translation
    return this.translate(key);
  }
}