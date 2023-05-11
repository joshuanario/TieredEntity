import React, { Component } from 'react';

export class Demo extends Component {
  static displayName = Demo.name;

  constructor(props) {
    super(props);
    this.state = { 
      selectedDataset: 'dutypositions',
      selectedTierStrategy: 'nestedset',
      aljobroles: [],
      loadingaljobroles: true,
      aldutypositions: [],
      loadingaldutypositions: true,
      nsjobroles: [],
      loadingnsjobroles: true,
      nsdutypositions: [],
      loadingnsdutypositions: true
    };
  }

  componentDidMount() {
    this.populateAlJobRoles();
    this.populateAlDutyPositions();
    this.populateNsJobRoles();
    this.populateNsDutyPositions();
  }

  static renderAdjacencyList(data) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <tbody>
          {data.map((gen, index1) =>
            <tr key={index1}>
              {gen.map((item, index2) => item?
              <td key={index2}>
                <span>{item.vertexId} </span>
                <span> {item.title} </span>
                <span> {item.nextId}</span>
              </td> : <td key={index2}>
                <span> </span>
              </td>)}
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  static renderNestedSet(data) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <tbody>
          {data.map((gen, index1) =>
            <tr key={index1}>
              {gen.map((item, index2) => item?
              <td key={index2}>
                <span>{item.vertexId} </span>
                <span> {item.title} </span>
                  <div>
                      [<span>{item.left}</span>,
                      <span> {item.right}</span>]
                  </div>
              </td> : <td key={index2}>
                <span> </span>
              </td>)}
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    const loading = this.state.loadingaldutypositions ||
      this.state.loadingaljobroles ||
      this.state.loadingnsdutypositions ||
      this.state.loadingnsjobroles
    let data = []
    let contents = <p><em>Loading...</em></p>
    if (!loading) {
      const dataset = this.state.selectedDataset
      const tierstrategy = this.state.selectedTierStrategy
      if (dataset === 'dutypositions') {
        if (tierstrategy === 'nestedset') {
          data = this.state.nsdutypositions
          contents = Demo.renderNestedSet(data)
        }
        if (tierstrategy === 'adjacencylist') {
          data = this.state.aldutypositions
          contents = Demo.renderAdjacencyList(data)
        }
      }
      if (dataset === 'jobroles') {
        if (tierstrategy === 'nestedset') {
          data = this.state.nsjobroles
          contents = Demo.renderNestedSet(data)
        }
        if (tierstrategy === 'adjacencylist') {
          data = this.state.aljobroles
          contents = Demo.renderAdjacencyList(data)
        }
      }
    }

    return (
      <div>
        <div>
          <label htmlFor='dataset'>Dataset</label><select name='dataset' onChange={e => {
            if (e.currentTarget) {
              this.setState({selectedDataset: e.currentTarget.value || 'dutypositions'})
            }
          }} value={this.state.selectedDataset}>
            <option value='jobroles'>
              Job Roles
            </option>
            <option value='dutypositions'>
              Duty Positions
            </option>
          </select>
        </div>
        <div>
          <label htmlFor='tierstrategy'>Tier Strategy</label><select name='tierstrategy' onChange={e => {
            if (e.currentTarget) {
              this.setState({selectedTierStrategy: e.currentTarget.value || 'nestedset'})
            }
          }} value={this.state.selectedTierStrategy}>
            <option value='nestedset'>
              Nested Set
            </option>
            <option value='adjacencylist'>
              Adjacency List
            </option>
          </select>
        </div>
        {contents}
      </div>
    );
  }

  async populateAlJobRoles() {
    const response = await fetch('api/jobroles/adjacencylist');
    const aljobroles = await response.json();
    this.setState({ aljobroles, loadingaljobroles: false });
  }

  async populateAlDutyPositions() {
    const response = await fetch('api/dutypositions/adjacencylist');
    const aldutypositions = await response.json();
    this.setState({ aldutypositions, loadingaldutypositions: false });
  }

  async populateNsJobRoles() {
    const response = await fetch('api/jobroles/nestedset');
    const nsjobroles = await response.json();
    this.setState({ nsjobroles, loadingnsjobroles: false });
  }

  async populateNsDutyPositions() {
    const response = await fetch('api/dutypositions/nestedset');
    const nsdutypositions = await response.json();
    this.setState({ nsdutypositions, loadingnsdutypositions: false });
  }
}
