import { makeAutoObservable } from "mobx";
import { CalculationModel } from "../models/calculationModel";

export class ViewModel {
  calculatedValue: CalculationModel = new CalculationModel("");

  constructor() {
    makeAutoObservable(this);
  }

  getNumber(): number {
    return this.calculatedValue.calculate();
  }
}
