import { makeAutoObservable } from "mobx";
import { CalculationModel } from "../models/calculationModel";

export class ViewModel {
  calculatedValue: CalculationModel = new CalculationModel("");
  calculatedString: string = "";

  constructor() {
    makeAutoObservable(this);
  }

  getCalculatedString(): string {
    return this.calculatedString;
  }

  increment(): number {
    return this.calculatedValue.increment();
  }

  add1 = (): void => {
    this.calculatedString += "1";
  };

  add2 = (): void => {
    this.calculatedString += "2";
  };

  add3 = (): void => {
    this.calculatedString += "3";
  };

  add4 = (): void => {
    this.calculatedString += "4";
  };

  add5 = (): void => {
    this.calculatedString += "5";
  };

  add6 = (): void => {
    this.calculatedString += "6";
  };

  add7 = (): void => {
    this.calculatedString += "7";
  };

  add8 = (): void => {
    this.calculatedString += "8";
  };

  add9 = (): void => {
    this.calculatedString += "9";
  };

  add0 = (): void => {
    this.calculatedString += "0";
  };

  addPlus = (): void => {
    this.calculatedString += "+";
  };

  addMinus = (): void => {
    this.calculatedString += "-";
  };

  addMultiply = (): void => {
    this.calculatedString += "*";
  };

  addDevide = (): void => {
    this.calculatedString += "/";
  };

  addLeftBracket = (): void => {
    this.calculatedString += "(";
  };
  addRightBracket = (): void => {
    this.calculatedString += ")";
  };
}
