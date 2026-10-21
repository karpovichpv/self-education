import { makeAutoObservable } from "mobx";
import { CalculationModel } from "../models/CalculationModel";

export class ViewModel {
  calculatedModel: CalculationModel = new CalculationModel("");
  calculatedString: string = "";

  constructor() {
    makeAutoObservable(this);
  }

  getCalculatedString(): string {
    return this.calculatedString;
  }

  calculate(): string {
    try {
      const result = this.calculatedModel.calculate(this.calculatedString);
      console.log(result);

      return result !== null && result !== undefined ? result.toString() : "";
    } catch (error) {
      console.error("Ошибка при вычислении", error);
      return `Ошибка`;
    }
  }

  addChar = (input: string): void => {
    this.calculatedString += input;
  };

  removeLast = (): void => {
    const handeledString = this.calculatedString.slice(
      0,
      this.calculatedString.length - 1,
    );
    this.calculatedString = handeledString;
  };

  addCe = (): void => {
    this.calculatedString = "";
  };
}
