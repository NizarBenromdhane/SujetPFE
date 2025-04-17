// Données de test (à remplacer par vos données réelles)
const resources = [
    { title: 'Les 10 compétences du manager moderne', description: 'Un article sur les compétences clés pour réussir en tant que manager.', image: 'article.jpg' },
    { title: 'Guide : Gestion de projet agile', description: 'Un guide pratique pour la gestion de projet agile.', image: 'guide.jpg' },
    // Ajoutez d'autres ressources ici
];

function displayResources() {
    const resourceList = document.querySelector('.resource-list');
    resourceList.innerHTML = '';

    resources.forEach(resource => {
        const resourceElement = document.createElement('div');
        resourceElement.className = 'resource';
        resourceElement.innerHTML = `
            <img src="${resource.image}" alt="${resource.title}">
            <h3>${resource.title}</h3>
            <p>${resource.description}</p>
            <button>${resource.buttonText || 'Lire la suite'}</button>
        `;
        resourceList.appendChild(resourceElement);
    });
}

// Fonction pour gérer le clic sur le bouton "Accéder au forum"
function goToForum() {
    // Implémentez la logique pour rediriger vers le forum
    console.log('Accéder au forum');
    // Exemple : window.location.href = '/forum';
}

// Initialisation de la page
displayResources();

// Écouteur d'événements pour le bouton "Accéder au forum"
document.querySelector('.forum button').addEventListener('click', goToForum);