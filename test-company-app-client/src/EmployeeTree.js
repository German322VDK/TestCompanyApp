import React, { useEffect, useState } from 'react';
import config from './config';

const EmployeeHierarchy = () => {
    const [employees, setEmployees] = useState(null);
    const apiUrl = `${config.DefaultApiUrl}/employees/getallemployed`; // Замените на ваш URL

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch(apiUrl);
                if (response.ok) {
                    const data = await response.json();
                    setEmployees(data);
                } else {
                    console.error("Ошибка при загрузке данных:", response.statusText);
                }
            } catch (error) {
                console.error("Ошибка при выполнении запроса:", error);
            }
        };

        fetchData();
    }, []);

    const colorSwitcher = (lvl) => {
        let colorClass;
    
        switch (lvl) {
            case 1:
                colorClass = "green";
                break;
            case 2:
                colorClass = "blue";
                break;
            case 3:
                colorClass = "orange";
                break;
            default:
                colorClass = "gray";
                break;
        }
    
        return colorClass;
    }

    const renderEmployeeTree = (employees, leaderId = null, level = 1) => {
        const subordinates = employees.filter(e => e.leaderId === leaderId);

        if (subordinates.length === 0) return null;
        let color = colorSwitcher(level);
        let colorParent = colorSwitcher(level - 1);
        
        return (
            <ul className={`tree ${color}-element-bl`}>
                {subordinates.map(subordinate => (
                    <li key={subordinate.id} className={`${color}-element-bg ${colorParent}-element-b`}>
                        <span className={`${color}-element-b employee-info ${color}-employee-info`}>
                            {`ID: ${subordinate.id} - ${subordinate.firstName} ${subordinate.firstName} ${subordinate.patronymic} (${subordinate.jobTitle}), LeaderId: ${subordinate.leaderId}`}
                        </span>
                        {renderEmployeeTree(employees, subordinate.id, level + 1)}
                    </li>
                ))}
            </ul>
        );
    };

    return (
        <div className="container">
            <h1>Иерархия сотрудников</h1>
            <div className="employee-tree container-tree">
                {employees ? renderEmployeeTree(employees) : <p>Загрузка данных...</p>}
            </div>
        </div>
    );
};

export default EmployeeHierarchy;
