document.addEventListener("DOMContentLoaded", function () {
    const addKpiBtn = document.getElementById("add-kpi");
    const kpiContainer = document.getElementById("kpi-container");
    const kpiDataInput = document.getElementById("kpiData");
    const pacForm = document.getElementById("pac-form");

    let kpiCounter = 0;

    addKpiBtn.addEventListener("click", function () {
        kpiCounter++;
        const kpiDiv = document.createElement("div");
        kpiDiv.classList.add("kpi-item");
        kpiDiv.innerHTML = `
            <div class="form-group">
                <label for="KPIValues[${kpiCounter - 1}].Nom">Nom du KPI:</label>
                <input type="text" class="form-input" name="KPIValues[${kpiCounter - 1}].Nom" required />
            </div>
            <div class="form-group">
                <label for="KPIValues[${kpiCounter - 1}].Valeur">Valeur du KPI:</label>
                <input type="text" class="form-input" name="KPIValues[${kpiCounter - 1}].Valeur" />
            </div>
            <button type="button" class="remove-kpi form-button"><i class="fas fa-minus-circle"></i> Supprimer</button>
        `;
        kpiContainer.appendChild(kpiDiv);

        const removeButton = kpiDiv.querySelector(".remove-kpi");
        removeButton.addEventListener("click", function () {
            kpiDiv.remove();
        });
    });

    pacForm.addEventListener("submit", function () {
        const kpis = [];
        kpiContainer.querySelectorAll(".kpi-item").forEach(item => {
            const nomInput = item.querySelector('input[name^="KPIValues"][name$="].Nom"]');
            const valeurInput = item.querySelector('input[name^="KPIValues"][name$="].Valeur"]');
            if (nomInput) {
                kpis.push({ Nom: nomInput.value, Valeur: valeurInput ? valeurInput.value : "" });
            }
        });
        kpiDataInput.value = JSON.stringify(kpis);
    });
});