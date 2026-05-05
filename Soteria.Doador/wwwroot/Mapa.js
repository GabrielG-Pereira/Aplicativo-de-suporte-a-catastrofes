window.mapaInterop = {
    mapa: null,

    // 1. Inicia o mapa em uma coordenada central
    iniciarMapa: function (idElemento, latInicial, lngInicial) {
        // Se já existir um mapa, ele destrói para criar um novo (evita erros)
        if (this.mapa !== null) {
            this.mapa.remove();
        }

        // Cria o mapa e define o zoom
        this.mapa = L.map(idElemento).setView([latInicial, lngInicial], 13);

        // Adiciona as "peças" visuais do mapa (ruas, bairros)
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap'
        }).addTo(this.mapa);
    },

    // 2. Adiciona os pinos dinâmicos recebidos do Backend
    adicionarPinos: function (locais) {
        // Cria um ícone customizado usando a imagem do seu projeto
        var iconeLaranja = L.icon({
            iconUrl: '/imagem/icone-localizacao.svg', // Coloque o caminho do seu ícone SVG ou PNG
            iconSize: [40, 40], // Tamanho do ícone
            iconAnchor: [20, 40], // Onde a "ponta" do ícone encosta no mapa
            popupAnchor: [0, -40]
        });

        // Para cada local que veio do banco de dados, cria um pino
        locais.forEach(local => {
            L.marker([local.latitude, local.longitude], { icon: iconeLaranja })
                .addTo(this.mapa)
                .bindPopup(`<b>${local.nome}</b><br>${local.tipo}`); // Balãozinho ao clicar
        });
    }
};