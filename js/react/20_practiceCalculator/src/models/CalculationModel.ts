import { makeAutoObservable } from "mobx";

export class CalculationModel {
  inputString: string;
  defaultNumber: number = 22;

  constructor(inputString: string) {
    this.inputString = inputString;
    makeAutoObservable(this);
  }

  calculate(): number {
    return this.defaultNumber;
  }

  increment(): number {
    this.defaultNumber = this.defaultNumber + 1;
      return this.defaultNumber ;
    }
}
