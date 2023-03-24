import React,{Component} from 'react';
import {variables} from './Variables.js';

//Creating Department class which extends reacts Component.
export class Department extends Component{
    
    //Creating a constructor with required values. 
    constructor(props){
        super(props);
        this.state={
            departments:[],
            modalTitle:"",
            DepartmentName:"",
            DepartmentId:0     
    }
}
    //Retrives the department data from the API and updates the state
    refreshList(){
        fetch(variables.API_URL+'department')
        .then(response=>response.json())
        .then(data=>{
            this.setState({departments:data});
        });
    
    }
    //this method is called when the component is added to page (DOM)
    componentDidMount(){
        this.refreshList();
    }
    //Updates Department name (event handler)
    changeDepartmentName =(e)=>{
        this.setState({DepartmentName:e.target.value});
    }

    // Method to set the state for adding a new department
    addClick(){
        this.setState({
            modalTitle:"Add Department",
            DepartmentId:0,
            DepartmentName:""
        });
    }
    // Method to set the state for editing a  department
    editClick(dep){
        this.setState({
            modalTitle:"Edit Department",
            DepartmentId:dep.DepartmentId,
            DepartmentName:dep.DepartmentName
        });
    }

    // Method to create new department by making a post request to the API
    createClick(){
        fetch(variables.API_URL+'department',{
            method:'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({
                DepartmentName:this.state.DepartmentName
            })
        })
        .then(res=>res.json())
        .then((result)=>{
            alert("Created");
            this.refreshList();
        },(error)=>{
            alert('Failed');
        })
    }
    // Method to update an existing department by making a post request to the API
    updateClick(){
        fetch(variables.API_URL+'department',{
            method:'PUT',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({
                DepartmentId:this.state.DepartmentId,
                DepartmentName:this.state.DepartmentName
            })
        })
        .then(res=>res.json())
        .then((result)=>{
            alert(result);
            this.refreshList();
        },(error)=>{
            alert('Failed');
        })
    }
    // Method to Delete an existing department by making a post request to the API
    deleteClick(id){
        console.log(id);
        console.log(variables.API_URL);
        if(window.confirm('Are you sure?')){
            fetch(variables.API_URL+'department/'+id,{
                    method:'DELETE',
                    headers:{
                        'Accept':'application/json',
                        'Content-Type':'application/json'
                    }
                })
                .then(res=>res.json())
                
                .then((result)=>{
                    alert(result);
                    this.refreshList();
                },(error)=>{
                    alert('Failed');
                })
        }
    }
    //The reder method is to display the component UI
    render(){
            const { departments, modalTitle, DepartmentId,DepartmentName } = this.state;
        return(
            <div>
                  {/* Button to open modal for adding a new department */}
                <button type="button"
                    className="btn btn-primary m-2 float-end"
                    data-bs-toggle="modal"
                    data-bs-target="#exampleModal"
                    onClick={()=>this.addClick()}>
                        Add Department
                    </button>
                     {/* Table to display the list of departments */}
                    <table className="table table-striped">
                        <thead>
                            <th>DepartmentID</th>
                            <th>DepartmentName</th>
                            <th>Options</th>
                        </thead>
                        <tbody>
                            {departments.map(dep=>
                                    <tr key ={dep.DepartmentId}>
                                    <td>{dep.DepartmentId}</td>
                                    <td>{dep.DepartmentName}</td>
                                    <td>
                                        {/* Button to open modal for editing the department */}
                                        <button type = "button" 
                                        className="btn btn-light mr-1"
                                                data-bs-toggle="modal"
                                                data-bs-target="#exampleModal"
                                                onClick={()=>this.editClick(dep)}>
                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-pencil-square" viewBox="0 0 16 16">
                                            <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"/>
                                            <path fillRule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z"/>
                                            </svg>
                                        </button>
                                         {/* Button to delete the department */}
                                        <button type = "button" 
                                        className="btn btn-light mr-1"
                                        onClick = {() => this.deleteClick(dep.DepartmentId)}>
                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-trash-fill" viewBox="0 0 16 16">
                                    <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z"/>
                                    </svg>
                                        </button>
                                    </td>
                                </tr>
                            )}
                        </tbody>                                
                        </table>
                {/* Modal for adding or editing a department */}
                <div className="modal fade" id="exampleModal" tabIndex="-1" aria-hidden="true">
                <div className="modal-dialog modal-lg modal-dialog-centered">
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title">{modalTitle}</h5>
                        <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"
                    ></button>
                </div>

                <div className="modal-body">
                    <div className="input-group mb-3">
                        <span className="input-group-text">DepartmentName</span>
                        <input type="text" className="form-control"
                        value={DepartmentName}
                        onChange={this.changeDepartmentName}/>
                    </div>
                    {/* Show Create button if DepartmentId is 0 (When creating dep) */}
                    {DepartmentId===0?
                        <button type="button"
                        className="btn btn-primary float-start"
                        onClick={()=>this.createClick()}
                        >Create</button>
                        :null}
                    {/* Show Update button if DepartmentId is not 0 (when edting dep)*/}
                    {DepartmentId!==0?
                        <button type="button"
                        className="btn btn-primary float-start"
                        onClick={()=>this.updateClick()}
                        >Update</button>
                        :null}
                </div>
            </div>
        </div>
     </div>
</div>
        )}
}