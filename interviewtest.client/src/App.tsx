import { useEffect, useState } from 'react';
import { Employee } from './Employee';

function App() {
    const [employees, setEmployees] = useState<Employee[]>([]);             
    const [newName, setNewName] = useState('');         
    const [newValue, setNewValue] = useState(-1);         
    const [editedName, setEditedName] = useState('');
    const [editedValue, setEditedValue] = useState(-1);   

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

    const onAdd = async () => {
        if (newName == "" || newName == "\n" || newValue < 0) return;

        await fetch(`/api/employees/add`, {
            method: 'POST', headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({                
                Id: 0,
                Name: newName,
                Value: newValue                
            }) })
        fetchEmployees()
        //reset
        setNewName(''); setNewValue(-1);
    }

    const onEdit = async (id: number) => {             
        if (editedName != "" && editedName != "\n" && editedValue > -1) {
            await fetch(`/api/employees/update`, {
                method: 'PUT', headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    Id: id,
                    Name: editedName,
                    Value: editedValue
                })
            })
        }
        await onCancel(id);
        fetchEmployees()
    }  

    const onEditClicked = async (id: number) => { 
        if (!confirm('Edit id ' + id + '?')) return;

        const element = document.getElementById('tdEdit' + id);
        if (element) {
            element.style.display = 'block';
            const elName = document.getElementById('tdEdit' + id + "-name");
            if (elName) {
                const oldName: string = elName.innerText;
                setEditedName(oldName);
            }
            
            const elValue = document.getElementById('tdEdit' + id + "-value");
            if (elValue) {
                const oldValue: number = Number(elValue.innerText);
                setEditedValue(oldValue);
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

    // TODO: Connectivity check line won't work if employees count is 0, ie a brand new company maybe?
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
                            <input id={`tdEdit${e.id}-name-new`} width={`100px`} title="Edited name, click to edit" value={editedName} onChange={(e) => setEditedName(e.target.value)} />
                            <input id={`tdEdit${e.id}-value-new`} width={`50px`} title="Edited value, click to edit" value={editedValue} onChange={(e) => setEditedValue(Number(e.target.value))} />
                            <button onClick={() => onEdit(e.id)}>Save Edit</button>
                            <button onClick={() => onCancel(e.id)}>Cancel Edit</button>
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
        <div>
            <div>Fill fields below for new employee</div>            
            <div>
                <div><div>Name: </div><input id="newName" title="New name" type="text" value={newName} onChange={(e) => setNewName(e.target.value)} /></div>
                <div><div>Value: </div><input id="newValue" title="New value" type="number" value={newValue} onChange={(e) => setNewValue(Number(e.target.value))} /></div>
            </div>
            then <button onClick={() => onAdd()}>Add Me!</button>
        </div>

        <div>
        <div>Increase name starts with E by 1, G by 10, any other by 100</div>
            <button onClick={() => onIncrease()}>Increase Me!</button>
            {
                sumABC <= maxABCLimit ? `` : `A B C bigger than eq ${maxABCLimit}: ${sumABC}`
            }
        </div>
    </>);

    
}

export default App;