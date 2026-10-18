import { makeAutoObservable } from "mobx";
import { evaluate } from "mathjs";

export class CalculationModel {
  inputString: string;
  defaultNumber: number = 22;

  constructor(inputString: string) {
    this.inputString = inputString;
    makeAutoObservable(this);
  }

  calculate(input: string): number | null {
    try {
      const result: number = evaluate(input);
      return result;
    } catch (error) {
      console.log(`Ошибка в математическом выражении`);
      return null;
    }
  }

  increment(): number {
    this.defaultNumber = this.defaultNumber + 1;
    return this.defaultNumber;
  }
}
