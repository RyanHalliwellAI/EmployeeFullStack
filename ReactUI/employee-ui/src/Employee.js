import React,{Component} from 'react';
import {variables} from './Variables.js';
//import { Department } from './Department.js';

export class Employee extends Component{
    
    //Creating a constructor with required values. 
    constructor(props){
        super(props);
        this.state={
            departments:[],
            employees:[],
            modalTitle:"",
            EmployeeId:0,
            EmployeeName:"",
            Department:"",
            DateOfJoining:"",
            PhotoFileName: "anonymous.png",
            PhotoPath:variables.PHOTO_URL
        
    }
}    
//Retrives the department and employee data from the API and updates the state
    refreshList(){
        fetch(variables.API_URL+'employee')
        .then(response=>response.json())
        .then(data=>{
            this.setState({employees:data});
        });
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
    // Changes employeeName state (event listener)
    changeEmployeeName =(e)=>{
        this.setState({EmployeeName:e.target.value});
    }
    // Changes department state (event listener)
    changeDepartment =(e)=>{
        this.setState({Department:e.target.value});
    }
    // Changes date of joining state (event listener)
    changeDateOfJoining =(e)=>{
        this.setState({DateOfJoining:e.target.value});
    }

    // Method to set the state for adding a new employee
    addClick(){
        this.setState({
            modalTitle:"Add Employee",
            EmployeeId:0,
            EmployeeName:"",
            Department:"IT",
            DateOfJoining:"",
            PhotoFileName: "anonymous.png"
        });
    }
    // Method to set the state for editing an employee
    editClick(emp){
        this.setState({
            modalTitle:"Edit Employee",
            EmployeeId:emp.EmployeeId,
            EmployeeName:emp.EmployeeName,
            Department:emp.Department,
            DateOfJoining:emp.DateOfJoining,
            PhotoFileName: emp.PhotoFileName
        });
    }
    // Method to create new employee by making a post request to the API
    createClick(){
        fetch(variables.API_URL+'employee',{
            method:'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({
                EmployeeName:this.state.EmployeeName,
                Department:this.state.Department,
                DateOfJoining:this.state.DateOfJoining,
                PhotoFileName: this.state.PhotoFileName           
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

    // Method to update an existing department by making a post request to the API
    updateClick(){
        fetch(variables.API_URL+'employee',{
            method:'PUT',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({
                EmployeeId:this.state.EmployeeId,
                EmployeeName:this.state.EmployeeName,
                Department:this.state.Department,
                DateOfJoining:this.state.DateOfJoining,
                PhotoFileName: this.state.PhotoFileName 
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
    // Method to Delete an existing employee by making a post request to the API
    deleteClick(id){
        console.log(id);
        console.log(variables.API_URL);

        if(window.confirm('Are you sure?')){
            fetch(variables.API_URL+'employee/'+id,{
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
    // image evant handler to save the file that is uploaded by user
    imageUpload=(e)=>{
        e.preventDefault();

        const formData=new FormData();
        formData.append("file",e.target.files[0],e.target.files[0].name);

        fetch(variables.API_URL+'employee/savefile',{
            method:'POST',
            body:formData
        })
        .then(res=>res.json())
        .then(data=>{
            this.setState({PhotoFileName:data});
        })
    }

    //The render method is to display the component UI
        render(){
            const { departments, employees, modalTitle, EmployeeId, EmployeeName, Department, DateOfJoining, PhotoPath, PhotoFileName } = this.state;

        return(
            <div>
            {/* Button to open modal for adding a new employee */}
            <button type="button"
                className="btn btn-primary m-2 float-end"
                data-bs-toggle="modal"
                data-bs-target="#exampleModal"
                onClick={()=>this.addClick()}>
                    Add Employee
                </button>
                        {/* Table to display the list of Employees */}
                        <table className="table table-striped">
                        <thead>
                                <th>EmployeeID</th>
                                <th>EmployeeName</th>
                                <th>Department</th>
                                <th>DOJ</th>
                                <th>Options</th>
                        </thead>
                        <tbody>
                        {employees.map(emp=>
                                <tr key ={emp.EmployeeId}>
                                <td>{emp.EmployeeId}</td>
                                <td>{emp.EmployeeName}</td>
                                <td>{emp.Department}</td>
                                <td>{emp.DateOfJoining}</td>
                                <td>
                                    {/* Button to open modal for editing the Employeee */}
                                    <button type = "button" 
                                    className="btn btn-light mr-1"
                                            data-bs-toggle="modal"
                                            data-bs-target="#exampleModal"
                                            onClick={()=>this.editClick(emp)}>
                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-pencil-square" viewBox="0 0 16 16">
                                        <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"/>
                                        <path fillRule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z"/>
                                        </svg>
                                    </button>
                                    {/* Button to open modal for Deleting the Employeee */}
                                    <button type = "button" 
                                    className="btn btn-light mr-1"
                                    onClick = {() => this.deleteClick(emp.EmployeeId)}>
                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-trash-fill" viewBox="0 0 16 16">
                                <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z"/>
                                </svg>
                                </button>
                                </td>
                                </tr>
                        )}
                        </tbody>                            
                    </table>
            {/* Modal for adding or editing a employee */}
            <div className="modal fade" id="exampleModal" tabIndex="-1" aria-hidden="true">
            <div className="modal-dialog modal-lg modal-dialog-centered">
            <div className="modal-content">
                <div className="modal-header">
                    <h5 className="modal-title">{modalTitle}</h5>
                    <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"
                ></button>
            </div>
            <div className="modal-body">
                 {/* Container for the form fields and image */}
                <div className = "d-flex flex-row bd-highlight mb-3">
                    {/* Input field for the employee name */}
                    <div className = "p-2 w-50 bd-highlight">
                        <div className="input-group mb-3">
                        <span className="input-group-text">Employee name</span>
                        <input type="text" className="form-control"
                        value={EmployeeName}
                        onChange={this.changeEmployeeName}/>
                    </div>
                    {/* Dropdown for the department selection*/}
                    <div className="input-group mb-3">
                        <span className="input-group-text">Department</span>
                        <select className="form-select"
                        onChange={this.changeDepartment}
                        value={Department}>
                            {departments.map(dep=><option key={dep.DepartmentId}>
                                {dep.DepartmentName}
                            </option>)}
                        </select>
                    </div>
                    {/* Input for the date of joining*/}
                    <div className="input-group mb-3">
                        <span className="input-group-text">DOJ</span>
                        <input type="date" className="form-control"
                        value={DateOfJoining}
                        onChange={this.changeDateOfJoining}/>
                    </div>
                    </div>
                {/*Display the selected file (or annoymos.png if not selected*/}
                    <div className="p-2 w-50 bd-highlight">
                        <img width="250px" height="250px" src={PhotoPath + PhotoFileName} alt= "test" />
                        <p>{PhotoPath+PhotoFileName}</p> {/* Display the PhotoFileName */}
                        <input className="m-2" type="file" onChange={this.imageUpload} />
                    </div>
                </div>
                {/*This is for creating a brand new employee*/}
                {EmployeeId===0?
                    <button type="button"
                    className="btn btn-primary float-start"
                    onClick={()=>this.createClick()}
                    >Create</button>
                    :null}
                {/*This is for editing a current employee*/}
                {EmployeeId!==0?
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
        )
    }
}