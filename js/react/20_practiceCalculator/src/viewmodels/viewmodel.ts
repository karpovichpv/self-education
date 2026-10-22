import { makeAutoObservable } from "mobx";
import { CalculationModel } from "../models/CalculationModel";

export class ViewModel {
  calculatedModel: CalculationModel = new CalculationModel();
  calculatedString: string = "";

  constructor() {
    makeAutoObservable(this);
  }

  get result(): string {
    return this.calculatedModel.evaluate(this.calculatedString);
  }

  addChar = (input: string): void => {
    this.calculatedString += input;
  };

  removeLast = (): void => {
    this.calculatedString = this.calculatedString.slice(0, -1);
  };

  clear = (): void => {
    this.calculatedString = "";
  };

  equals = (): void => {
    const value = this.result;

    if (value !== "" && value !== "Ошибка") {
      this.calculatedString = value;
    }
  };
}
