import React, { Component } from 'react';
import { FormGroup } from 'reactstrap';

export class EquationEvaluation extends Component {
    static displayName = EquationEvaluation.name;

    constructor(props) {
        super(props);
        this.state = { responseData: [], equation: "", xValue: 0, yValue: 0, loading: true };

        this.persistEquation = this.persistEquation.bind(this);
        this.persistXValue = this.persistXValue.bind(this);
        this.persistYValue = this.persistYValue.bind(this);
        this.executeRequest = this.executeRequest.bind(this);
    }

    componentDidMount() {
        //this.executeRequest();
    }

    static renderEvaluationResults(response) {
        return (
            <div className="card">
                <div className="card-body">
                    <h5>Your equation was evaulated.</h5>
                    <div className="card">
                        <div className="card-body">
                            <ul>
                                Adjusted Equation: {response.adjustedExpression}
                            </ul>
                            <ul>
                                Value: {response.valueGenerated}
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : EquationEvaluation.renderEvaluationResults(this.state.responseData);

        return (
            <div>
                <h1 id="tabelLabel" >Math Expression Evaluation</h1>
                <form>
                    <div className="form-group">
                        <label for="equation">Math Equation</label>
                        <input required type="text" className="form-control" placeholder="2+2" value={this.state.equation} onChange={this.persistEquation} />
                    </div>
                    <div className="form-group">
                        <label for="equation">X Variable</label>
                        <input required type="text" className="form-control" placeholder="2+2" value={this.state.xValue} onChange={this.persistXValue} />
                    </div>
                    <div className="form-group">
                        <label for="equation">y Variable</label>
                        <input required type="text" className="form-control" placeholder="2+2" value={this.state.yValue} onChange={this.persistYValue} />
                    </div>
                    <button variant="primary" onClick={this.executeRequest} type="button">Eval</button>
                </form>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
        );
    }

    persistEquation(event) {
        this.setState({ equation: event.target.value });
    }

    persistXValue(event) {
        this.setState({ xValue: parseFloat(event.target.value) });
    }

    persistYValue(event) {
        this.setState({ yValue: parseFloat(event.target.value) });
    }

    async executeRequest() {
        const response = await fetch(
            'https://mathfun.azurewebsites.net/MathExpressions/Evaluate',
            {
                method: 'POST',
                headers: { Accept: 'application/json', 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    expression: this.state.equation,
                    variableEntries: [
                        { name: 'x', value: this.state.xValue },
                        { name: 'y', value: this.state.yValue }
                    ]
                })
            });
        const data = await response.json();
        this.setState({ responseData: data, xValue: this.state.xValue, yValue: this.state.yValue, equation: this.equation, loading: false });
    }
}
