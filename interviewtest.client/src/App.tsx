import { useEffect, useState } from 'react';
import { Employee } from './Employee';

function App() {
    const [employees, setEmployees] = useState<Employee[]>([]);         
    const maxABCLimit: number = 11171;

    useEffect(() => {
        fetchEmployees();
    }, []);

    async function fetchEmployees() {
        const response = await fetch('api/employees');
        const data = await response.json();                
        setEmployees(data);
    }

    const onDelete = async (id: number) => {
        if (!confirm('Delete id ' + id + '?')) return
        await fetch(`/api/employees/${id}`, { method: 'DELETE' })
        fetchEmployees()
    }

    const onIncrease = async () => {        
        await fetch(`/api/employees/increase`, { method: 'GET' })
        fetchEmployees()
    }

    const onAdd = async () => { // TODO
        await fetch(`/api/employees/add`, {
            method: 'POST', headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({                
                    Id: 0,
                    Name: 'Hello',
                    Value: 900                
            }) })
        fetchEmployees()
    }

    const onEdit = async (id: number) => {     
        let newName: string = "";
        let newValue: number = 0;

        const elNameNew = document.getElementById('tdEdit' + id + "-name-new");
        if (elNameNew) {
            newName = elNameNew.innerText;
        }               

        const elValueNew = document.getElementById('tdEdit' + id + "-value-new");
        if (elValueNew && !Number.isNaN(elValueNew.innerText)) {
            newValue = Number(elValueNew.innerText);
        }               

        if (newName != "" && newName != "\n" && newValue > 0) {
            await fetch(`/api/employees/update`, {
                method: 'PUT', headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    Id: id,
                    Name: newName,
                    Value: newValue
                })
            })
        }
        await onCancel(id);
        fetchEmployees()
    }  

    const onEditClicked = async (id: number) => { // show hide buttons
        if (!confirm('Edit id ' + id + '?')) return;

        const element = document.getElementById('tdEdit' + id);
        if (element) {
            element.style.display = 'block';
            const elName = document.getElementById('tdEdit' + id + "-name");
            if (elName) {
                const elNameNew = document.getElementById('tdEdit' + id + "-name-new");
                if (elNameNew) {
                    elNameNew.innerText = elName.innerText;
                    elNameNew.contentEditable = "true";
                    elNameNew.style.backgroundColor = "lightblue";
                }               
            }
            
            const elValue = document.getElementById('tdEdit' + id + "-value");
            if (elValue) {                
                const elValueNew = document.getElementById('tdEdit' + id + "-value-new");
                if (elValueNew) {
                    elValueNew.innerText = elValue.innerText;
                    elValueNew.contentEditable = "true";
                    elValueNew.style.backgroundColor = "lightblue";
                }                
            }
        }

        // hide all other buttons TODO
    }  

    const onCancel = async (id: number) => { // hide edit save cancel buttons, restore    
        const element = document.getElementById('tdEdit' + id);
        if (element) {
            element.style.display = 'none';            
        }           
    }      
    
    const sumABC = employees.filter((e) => e.name.startsWith("A") || e.name.startsWith("B") || e.name.startsWith("C")).reduce((prev, curr) => prev + curr.value, 0);

    return (<>
        <div>Connectivity check: {employees.length > 0 ? `OK (${employees.length})` : `NOT READY`}</div>
        <div>
            <table>
                <thead><tr><th>Actions</th><th>Name</th><th>Value</th></tr></thead>
                <tbody>
                {employees.map(e => (
                    <tr key={e.id}>
                        <td>
                            <button onClick={() => onEditClicked(e.id)}>Edit</button>
                            <button onClick={() => onDelete(e.id)}>Delete</button>
                        </td>
                        <td id={`tdEdit${e.id}-name`}>{e.name}</td>
                        <td id={`tdEdit${e.id}-value`}>{e.value}</td>
                        <td style={{ display: "none" }} id={`tdEdit${e.id}`}>
                            <td id={`tdEdit${e.id}-name-new`}></td>
                            <td id={`tdEdit${e.id}-value-new`}></td>
                            <button onClick={() => onEdit(e.id)}>Save Edit</button>
                            <button onClick={() => onCancel(e.id)}>Cancel Edit</button>
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
        <div><button onClick={() => onAdd()}>Add Me! TODO</button>
        <div>
                <div>Name: </div><div id="newName">NewName</div><div id="newValue">0</div>
            </div>
        </div>
        <div>
            <button onClick={() => onIncrease()}>Increase Me!</button>
            {
                sumABC <= maxABCLimit ? `` : `A B C bigger than eq ${maxABCLimit}: ${sumABC}`
            }                
        </div>
    </>);

    
}

export default App;