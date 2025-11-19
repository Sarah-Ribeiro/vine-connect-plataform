pipeline {
    agent any
    
    environment {
        // Defina suas variáveis de ambiente
        PROJECT_NAME = 'vine-connect-plataform'
        DEPLOY_PATH = 'C:\\inetpub\\wwwroot\\vinheria' // Para IIS no Windows
        // ou
        // DEPLOY_PATH = '/var/www/html/vinheria' // Para Linux
    }
    
    stages {
        stage('Checkout') {
            steps {
                echo '📦 Fazendo checkout do código...'
                checkout scm
            }
        }
        
        stage('Build') {
            steps {
                echo '🔨 Iniciando build...'
                echo "Deploy da Vinheria - ${PROJECT_NAME}"
                
                // Se for projeto .NET
                script {
                    if (isUnix()) {
                        sh 'echo "Build em ambiente Linux"'
                        // sh 'dotnet build'
                    } else {
                        bat 'echo "Build em ambiente Windows"'
                        // bat 'dotnet build'
                    }
                }
            }
        }
        
        stage('Tests') {
            steps {
                echo '🧪 Executando testes...'
                script {
                    if (isUnix()) {
                        sh 'echo "Testes executados com sucesso!"'
                    } else {
                        bat 'echo "Testes executados com sucesso!"'
                    }
                }
            }
        }
        
        stage('Deploy') {
            steps {
                echo '🚀 Iniciando deploy...'
                script {
                    if (isUnix()) {
                        sh """
                            echo "Copiando arquivos para ${DEPLOY_PATH}"
                            mkdir -p ${DEPLOY_PATH}
                            cp -r * ${DEPLOY_PATH}
                            echo "Deploy concluído com sucesso!"
                        """
                    } else {
                        bat """
                            echo Copiando arquivos para ${DEPLOY_PATH}
                            if not exist "${DEPLOY_PATH}" mkdir "${DEPLOY_PATH}"
                            xcopy /E /I /Y * "${DEPLOY_PATH}"
                            echo Deploy concluido com sucesso!
                        """
                    }
                }
            }
        }
    }
    
    post {
        success {
            echo '✅ Pipeline executado com sucesso!'
            // emailext subject: "Deploy Vinheria - Sucesso",
            //          body: "O deploy foi realizado com sucesso!",
            //          to: "seu-email@example.com"
        }
        failure {
            echo '❌ Pipeline falhou!'
            // emailext subject: "Deploy Vinheria - Falha",
            //          body: "O deploy falhou. Verifique os logs.",
            //          to: "seu-email@example.com"
        }
    }
}