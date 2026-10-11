export class CalculationModel {
  inputString: string;

  constructor(inputString: string) {
    this.inputString = inputString;
  }

  calculate(): number {
    return 20;
  }
}
