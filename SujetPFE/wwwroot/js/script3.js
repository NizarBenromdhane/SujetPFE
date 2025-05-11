document.addEventListener('DOMContentLoaded', function () {
    // Exemple de données dynamiques (remplacez avec vos données réelles)
    const data = {
        rdvCount: 4,
        rdvAverage: 4,
        visitCounts: [10, 5, 8, 12],
        objectifs: 5
    };

    // Fonction pour calculer le TRO
    function calculateTRO(rdvCount, objectifs) {
        if (objectifs === 0) {
            return 0;
        }
        return (rdvCount / objectifs) * 100;
    }

    // Calcul du TRO
    const tro = calculateTRO(data.rdvCount, data.objectifs);
    const troValue = tro.toFixed(2);

    // Mettre à jour les informations générales
    const rdvCountElement = document.getElementById('rdv-count');
    if (rdvCountElement) {
        rdvCountElement.textContent = data.rdvCount;
    } else {
        console.error("Element with ID 'rdv-count' not found.");
    }

    const rdvAverageElement = document.getElementById('rdv-average');
    if (rdvAverageElement) {
        rdvAverageElement.textContent = data.rdvAverage;
    } else {
        console.error("Element with ID 'rdv-average' not found.");
    }

    const troValueElement = document.getElementById('tro-value');
    if (troValueElement) {
        troValueElement.textContent = troValue + '%';
    } else {
        console.error("Element with ID 'tro-value' not found.");
    }

    // Mettre à jour les nombres de visites dans le tableau
    const visitCounts = document.querySelectorAll('.visit-count');
    visitCounts.forEach((count, index) => {
        if (data.visitCounts[index] !== undefined) {
            count.textContent = data.visitCounts[index];
        }
    });

    // Gestion des filtres (exemple)
    const dateStart = document.getElementById('date-range-start');
    const dateEnd = document.getElementById('date-range-end');
    const directionSelect = document.getElementById('direction');
    const agentSelect = document.getElementById('agent');

    function applyFilters() {
        console.log('Filtres appliqués');
    }

    dateStart.addEventListener('change', applyFilters);
    dateEnd.addEventListener('change', applyFilters);
    directionSelect.addEventListener('change', applyFilters);
    agentSelect.addEventListener('change', applyFilters);

    // Gestion des boutons de détails (exemple)
    const detailButtons = document.querySelectorAll('.details-btn');
    detailButtons.forEach(button => {
        button.addEventListener('click', function () {
            console.log('Détails cliqués');
        });
    });

    // Gestion des actions Rendez-vous
    document.getElementById('add-rdv').addEventListener('click', function () {
        console.log('Ajouter un RDV');
        window.location.href = '/SuiviIRC/AjouterRDV';
    });

    // document.getElementById('modify-rdv').addEventListener('click', function () {
    //     console.log('Modifier un RDV');
    //     // Récupérez l'ID du RDV sélectionné à partir de votre interface utilisateur
    //     const rdvId = getSelectedRdvId(); // Remplacez getSelectedRdvId() par votre fonction de récupération d'ID
    //     if (rdvId) {
    //         window.location.href = `/SuiviIRC/ModifierRDV/${rdvId}`;
    //     } else {
    //         console.error("ID du RDV non trouvé.");
    //         // Affichez un message d'erreur à l'utilisateur (par exemple, une alerte)
    //         alert("Veuillez sélectionner un RDV.");
    //     }
    // });

    // document.getElementById('report-rdv').addEventListener('click', function () {
    //     console.log('Compte Rendu');
    //     // Récupérez l'ID du RDV sélectionné à partir de votre interface utilisateur
    //     const rdvId = getSelectedRdvId(); // Remplacez getSelectedRdvId() par votre fonction de récupération d'ID
    //     if (rdvId) {
    //         window.location.href = `/SuiviIRC/CompteRendu/${rdvId}`;
    //     } else {
    //         console.error("ID du RDV non trouvé.");
    //         // Affichez un message d'erreur à l'utilisateur (par exemple, une alerte)
    //         alert("Veuillez sélectionner un RDV.");
    //     }
    // });

    // Fonction pour récupérer l'ID du RDV sélectionné (à implémenter)
    function getSelectedRdvId() {
        // Implémentez votre logique pour récupérer l'ID du RDV sélectionné
        // Par exemple, si vous avez un tableau de RDV, vous pouvez récupérer l'ID de la ligne sélectionnée
        // ou si vous avez un champ caché, vous pouvez récupérer sa valeur.
        // Retournez l'ID du RDV ou null si aucun RDV n'est sélectionné.
        // Exemple (à adapter à votre cas) :
        // const selectedRow = document.querySelector('.rdv-row.selected');
        // if (selectedRow) {
        //     return selectedRow.dataset.rdvId;
        // }
        return null; // Remplacez par votre logique
    }
});