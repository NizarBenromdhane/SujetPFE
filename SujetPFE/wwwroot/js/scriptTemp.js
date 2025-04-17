// Données de test (à remplacer par vos données réelles)
const templates = [
    { id: 1, name: 'Modèle Client 1', type: 'type1', date: '2025-03-01', icon: 'file-alt' },
    { id: 2, name: 'Modèle Client 2', type: 'type2', date: '2025-03-02', icon: 'file-invoice' },
    { id: 3, name: 'Modèle Client 3', type: 'type1', date: '2025-03-03', icon: 'file-alt' },
    { id: 4, name: 'Modèle Client 4', type: 'type2', date: '2025-03-04', icon: 'file-invoice' },
    { id: 5, name: 'Modèle Client 5', type: 'type1', date: '2025-03-05', icon: 'file-alt' },
    { id: 6, name: 'Modèle Client 6', type: 'type2', date: '2025-03-06', icon: 'file-invoice' },
];

function displayTemplates(templates) {
    const templateList = document.getElementById('templateList');
    templateList.innerHTML = '';

    templates.forEach(template => {
        const card = document.createElement('div');
        card.className = 'template-card';
        card.innerHTML = `
            <h2><i class="fas fa-${template.icon}"></i> ${template.name}</h2>
            <p>Date de création : ${template.date}</p>
            <div class="actions">
                <button onclick="downloadTemplate(${template.id})"><i class="fas fa-download"></i></button>
                <button onclick="editTemplate(${template.id})"><i class="fas fa-edit"></i></button>
                <button onclick="deleteTemplate(${template.id})"><i class="fas fa-trash-alt"></i></button>
            </div>
        `;
        templateList.appendChild(card);
    });
}

function downloadTemplate(id) {
    console.log(`Télécharger le modèle ${id}`);
    // Exemple : window.location.href = `/api/templates/${id}/download`;
}

function editTemplate(id) {
    console.log(`Modifier le modèle ${id}`);
    // Exemple : window.location.href = `/templates/${id}/edit`;
}

function deleteTemplate(id) {
    console.log(`Supprimer le modèle ${id}`);

    const index = templates.findIndex(template => template.id === id);
    if (index !== -1) {
        templates.splice(index, 1);
        displayTemplates(templates);
    }
}

const searchInput = document.getElementById('search');
const filterTypeSelect = document.getElementById('filterType');

function applyFilters() {
    const searchTerm = searchInput.value.toLowerCase();
    const filterType = filterTypeSelect.value;

    let filteredTemplates = templates.filter(template => {
        if (!template.name.toLowerCase().includes(searchTerm)) return false;
        if (filterType !== 'all' && template.type !== filterType) return false;
        return true;
    });

    displayTemplates(filteredTemplates);
}

searchInput.addEventListener('input', applyFilters);
filterTypeSelect.addEventListener('change', applyFilters);

document.getElementById('createTemplate').addEventListener('click', () => {
    console.log('Créer un nouveau modèle');
    // Exemple : window.location.href = '/templates/create';
});

displayTemplates(templates);