node {
  stage('SCM') {
    checkout scm
  }
  stage('Build Stage') {
    bat 'dotnet build'
  }
  stage('SonarQube Analysis') {
    def scannerHome = tool 'SonarScanner for MSBuild'
    withSonarQubeEnv() {
      bat "dotnet ${scannerHome}\\SonarScanner.MSBuild.dll begin /k:\"bookshop\""
      bat "dotnet build"
      bat "dotnet ${scannerHome}\\SonarScanner.MSBuild.dll end"
    }
  }
  stage('Unit Test Stage') {
    bat 'dotnet test --filter FullyQualifiedName~Tests.UnitTests'
  }
  stage('API Test Stage') {
     bat 'dotnet test --filter FullyQualifiedName~Tests.ApiTests'
  }
}
