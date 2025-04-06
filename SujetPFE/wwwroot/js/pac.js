document.addEventListener("DOMContentLoaded", function () {
    const addActionButton = document.getElementById("add-action");
    const kpiTableBody = document.getElementById("kpi-table-body");
    const addKpiBtn = document.getElementById("add-kpi-btn");
    const addKpiForm = document.getElementById("add-kpi-form");
    const saveKpiBtn = document.getElementById("save-kpi-btn");
    const newKpiNameInput = document.getElementById("new-kpi-name");
    const newKpiValueInput = document.getElementById("new-kpi-value");

    addActionButton.addEventListener("click", function () {
        const newAction = prompt("Veuillez entrer une nouvelle action :");
        if (newAction) {
            alert("Action ajoutée : " + newAction);
        }
    });

    addKpiBtn.addEventListener("click", function () {
        addKpiForm.style.display = "block";
    });

    saveKpiBtn.addEventListener("click", function () {
        const kpiName = newKpiNameInput.value;
        const kpiValue = newKpiValueInput.value;
        if (kpiName && kpiValue) {
            addKpiRow(kpiName, kpiValue);
            newKpiNameInput.value = "";
            newKpiValueInput.value = "";
            addKpiForm.style.display = "none";
        }
    });

    function createButton(iconClass, onClickHandler) {
        const button = document.createElement("button");
        button.classList.add("action-btn");
        button.innerHTML = `<i class="${iconClass}"></i>`;
        button.addEventListener("click", onClickHandler);
        return button;
    }

    function addKpiRow(kpiName, value) {
        const row = document.createElement("tr");
        row.classList.add("kpi-row");

        const kpiCell = document.createElement("td");
        kpiCell.textContent = kpiName;

        const valueCell = document.createElement("td");
        valueCell.textContent = value;

        const actionCell = document.createElement("td");

        const editButton = createButton("fas fa-edit", function () {
            const newValue = prompt(`Modifier ${kpiName}:`, valueCell.textContent);
            if (newValue) {
                valueCell.textContent = newValue;
            }
        });

        const deleteButton = createButton("fas fa-trash", function () {
            if (confirm("Voulez-vous supprimer ce KPI ?")) {
                row.remove();
            }
        });

        actionCell.appendChild(editButton);
        actionCell.appendChild(deleteButton);

        row.appendChild(kpiCell);
        row.appendChild(valueCell);
        row.appendChild(actionCell);

        kpiTableBody.appendChild(row);
    }

    addKpiRow("KPI 1", "Valeur 1");
    addKpiRow("KPI 2", "Valeur 2");

    function attachEventListeners() {
        document.querySelectorAll(".edit-btn").forEach(button => {
            button.addEventListener("click", function () {
                const row = this.closest("tr");
                const valueCell = row.querySelector("td:nth-child(2)");
                const newValue = prompt("Modifier KPI:", valueCell.textContent);
                if (newValue) {
                    valueCell.textContent = newValue;
                }
            });
        });

        document.querySelectorAll(".delete-btn").forEach(button => {
            button.addEventListener("click", function () {
                if (confirm("Voulez-vous supprimer ce KPI ?")) {
                    this.closest("tr").remove();
                }
            });
        });
    }

    attachEventListeners();
});