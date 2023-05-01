import React, { Component } from 'react';

export class Demo extends Component {
  static displayName = Demo.name;

  constructor(props) {
    super(props);
    this.state = { 
      aljobroles: [],
      loadingaljobroles: true,
      aldutypositions: [],
      loadingaldutypositions: true,
      nsjobroles: [],
      loadingnsjobroles: true,
      nsdutypositions: [],
      loadingnsdutypositions: true
    };
    this.datasetref = React.createRef();
    this.tierstrategyref = React.createRef();
  }

  componentDidMount() {
    this.populateAlJobRoles();
    this.populateAlDutyPositions();
    this.populateNsJobRoles();
    this.populateNsDutyPositions();
  }

  static renderNestedSet(data) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <tbody>
          {data.map((gen, index1) =>
            <tr key={index1}>
              {gen.map((item, index2) => 
              <td key={index2}>
                <span>{item.VertexId}</span>
                <span>{item.Title}</span>
                <span>{item.NextId}</span>
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
              {gen.map((item, index) => 
              <td key={index}>
                <span>{item.VertexId}</span>
                <span>{item.Title}</span>
                  <div>
                      [<span>{item.Left}</span>,
                      <span>{item.Right}</span>]
                  </div>
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
    if (!loading && !this.datasetref.current && !this.tierstrategyref.current) {
      const datasetref = this.datasetref.current
      const tierstrategyref = this.tierstrategyref.current
      const dataset = datasetref.value || 'dutypositions'
      const tierstrategy = tierstrategyref.value || 'nestedset'
      if (dataset == 'dutypositions') {
        if (tierstrategy == 'nestedset') {
          data = this.state.nsdutypositions
        }
        if (tierstrategy == 'adjacencylist') {
          data = this.state.aldutypositions
        }
      }
      if (dataset == 'jobroles') {
        if (tierstrategy == 'nestedset') {
          data = this.state.nsjobroles
        }
        if (tierstrategy == 'adjacencylist') {
          data = this.state.aljobroles
        }
      }
    }
    const contents = loading
      ? <p><em>Loading...</em></p>
      : Demo.renderNestedSet(data);

    return (
      <div>
        <div>
          <label htmlFor='dataset'>Dataset</label><select ref={this.datasetref} name='dataset'>
            <option value='dutypositions'>
              Duty Positions
            </option>
            <option value='jobroles'>
              Job Roles
            </option>
          </select>
        </div>
        <div>
          <label htmlFor='tierstrategy'>Tier Strategy</label><select ref={this.tierstrategyref} name='tierstrategy'>
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
