using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

/* Modelica SysBio --> SBML translator
Made for purposes of SysBio translation thesis
Mentors: Miha Moškon, Miha Mraz

Faculty of Computer and Information Science Ljubljana

Timotej Osolin

Please read the attached README.txt file! 
*/

namespace Diploma
{
    public partial class MainWindow : Form
    {
        //variables
        public static String compartment_name = "compartment";
        public static bool reversibility_ESReaction = false;
        public static bool reversibility_Source = false;
        public static bool reversibility_GenExp = false;
        public static bool reversibility_EFormation = false;
        public static bool reversibility_Elimination = false;


        public MainWindow()
        {
            InitializeComponent();
        }
        //About handler
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().Show();

        }
        //Settings handler
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Settings().ShowDialog();
        }
        //Save button handler
        private void btn_save_Click(object sender, EventArgs e)
        {
            SaveFileDialog shraniDat = new SaveFileDialog();
            DialogResult potrditev;

            shraniDat.Filter = "SBML Files (.xml)|*.xml";
            shraniDat.Title = "Choose a file to save the SBML translation to";
            shraniDat.FileName = "translation.xml";

            potrditev = shraniDat.ShowDialog();
            if(potrditev == DialogResult.OK)
            {
                tb_save.Text = shraniDat.FileName;
                label_excl_save.Visible = false;
            }
        }
        //Open button handler
        private void btn_open_Click(object sender, EventArgs e)
        {
            OpenFileDialog odpriDat = new OpenFileDialog();
            DialogResult potrditev;

            //Filters for opening files - .mo (all) - we can only parse one file at a time
            odpriDat.Filter = "Modelica Files (.mo)|*.mo|All Files (*.*)|*.*";
            odpriDat.Multiselect = false;

            //Open and save in textbox - this is read by parse button handler
            potrditev = odpriDat.ShowDialog();
            if (potrditev == DialogResult.OK)
            {
                tb_open.Text = odpriDat.FileName;
                label_excl_open.Visible = false;
            }
        }
        //Parsing button handler
        private void btn_parse_Click(object sender, EventArgs e)
        {
            ArrayList datoteka = new ArrayList();
            bool prekini = false;
            String fileOpen = tb_open.Text;
            String fileSave = tb_save.Text;
            String saveString;
            if (fileSave == "")
            {
                label_excl_save.Visible = true;
                prekini = true;
            }
            if (fileOpen == "")
            {
                label_excl_open.Visible = true;
                prekini = true;
            }
            if (prekini)
            {
                return;
            }
            else
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(fileOpen);
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    datoteka.Add(line);
                }
                reader.Close();
                saveString = parse(datoteka);
                System.IO.File.WriteAllText(tb_save.Text, saveString);

                tb_statistics.Text += "Successfully saved!";

            }


        }

        //Menu open file handler
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btn_open_Click(sender, e);
        }
        //Menu save file handler
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btn_save_Click(sender, e);
        }
        //Exit handler
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //How to use handler
        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new How_to_use().ShowDialog();
        }


        private String parse(ArrayList datoteka)
        {

            Model_statistics model = new Model_statistics();

            ArrayList Protein_list = new ArrayList();
            ArrayList mRNA_list = new ArrayList();
            ArrayList Metabolite_list = new ArrayList();
            ArrayList Enzyme_list = new ArrayList();
            ArrayList Source_list = new ArrayList();
            ArrayList ESReaction_list = new ArrayList();
            ArrayList EFormation_list = new ArrayList();
            ArrayList GeneExpression_list = new ArrayList();
            ArrayList Elimination_list = new ArrayList();
            ArrayList GeneExpression_pos_list = new ArrayList();
            ArrayList GeneExpression_neg_list = new ArrayList();
            ArrayList GeneExpression_nor_list = new ArrayList();
            ArrayList EElimination_list = new ArrayList();
            ArrayList CEElimination_list = new ArrayList();
            ArrayList ESource_list = new ArrayList();
            ArrayList CMass_list = new ArrayList();

            int numOfConnections = 0;
                        

            String line = "";
            String word1 = ""; //word 1 - species, reaction
            String word2 = ""; //word 2 - name of word1
            String tmpword3 = ""; //word 3  - word inside parenthesis for parsing connects
            String param1 = ""; //parameter 1 of connect function
            String param2 = ""; //parameter 2 of connect function

           // bool done = false;

            //Reading from file
            for (int i = 0; i < datoteka.Count; i++) //iterate through all lines
            {
                //trim line leading and trailing spaces
                line = (String)datoteka[i];
                line = line.Trim();
                word1 = ""; //reset word 1 
                word2 = ""; //reset word 2
                tmpword3 = "";
                param1 = "";
                param2 = "";

                //split on space
                word1 = line.Split(' ')[0];
                if (line.Split(' ').Length > 1) // is there a second word?
                    word2 = line.Split(' ')[1];

                //check if we have first parenthesis - useful for connects later on - define tmpword3
                if (word1.Split('(').Length > 1)
                {
                    tmpword3 = line.Split('(')[1];
                    tmpword3 = tmpword3.Split(')')[0];
                }


                //split on ( - get name, remove all parameters                
                word1 = word1.Split('(')[0];
                word2 = word2.Split('(')[0];


                //----------Parse 1st word 
                //link it to correct class
                if (word1.Equals("model"))
                {
                    model.name = word2;
                }
                else if (word1.Equals("SysBio.Source"))
                {
                    Source tmpSource = new Source();
                    tmpSource.name = word2;
                    Source_list.Add(tmpSource);
                }
                else if (word1.Equals("SysBio.Cmass"))
                {
                    CMass tmpCmass = new CMass();
                    tmpCmass.name = word2;
                    CMass_list.Add(tmpCmass);
                }
                else if (word1.Equals("SysBio.Metabolite"))
                {
                    Metabolite tmpMetabolite = new Metabolite();
                    tmpMetabolite.name = word2;
                    Metabolite_list.Add(tmpMetabolite);
                }
                else if (word1.Equals("SysBio.Enzyme"))
                {
                    Enzyme tmpEnzyme = new Enzyme();
                    tmpEnzyme.name = word2;
                    Enzyme_list.Add(tmpEnzyme);
                }
                else if (word1.Equals("SysBio.mRNA"))
                {
                    mRNA tmpmRNA = new mRNA();
                    tmpmRNA.name = word2;
                    mRNA_list.Add(tmpmRNA);
                }
                else if (word1.Equals("SysBio.Protein"))
                {
                    Protein tmpProtein = new Protein();
                    tmpProtein.name = word2;
                    Protein_list.Add(tmpProtein);
                }
                else if (word1.Equals("SysBio.EFormation_lin"))
                {
                    EFormation_lin tmpEForm = new EFormation_lin();
                    tmpEForm.name = word2;
                    EFormation_list.Add(tmpEForm);
                }
                else if (word1.Equals("SysBio.GeneExpressionControl_lin"))
                {
                    GenExpression_lin tmpGenExp = new GenExpression_lin();
                    tmpGenExp.name = word2;
                    GeneExpression_list.Add(tmpGenExp);
                }
                else if (word1.Equals("SysBio.ESReaction"))
                {
                    ESReaction tmpESReaction = new ESReaction();
                    tmpESReaction.name = word2;
                    ESReaction_list.Add(tmpESReaction);
                }
                else if (word1.Equals("SysBio.NElimination"))
                {
                    Elimination tmpElimination = new Elimination();
                    tmpElimination.name = word2;
                    Elimination_list.Add(tmpElimination);
                }
                else if (word1.Equals("SysBio.GeneExpressionControl"))
                {
                    GenExpression tmpgen1 = new GenExpression();
                    tmpgen1.name = word2;
                    GeneExpression_nor_list.Add(tmpgen1);
                }
                else if (word1.Equals("SysBio.GeneExpressionControl_negative"))
                {
                    GenExpression_neg tmpgen2 = new GenExpression_neg();
                    tmpgen2.name = word2;
                    GeneExpression_neg_list.Add(tmpgen2);
                }
                else if (word1.Equals("SysBio.GeneExpressionControl_positive"))
                {
                    GenExpression_pos tmpgen3 = new GenExpression_pos();
                    tmpgen3.name = word2;
                    GeneExpression_neg_list.Add(tmpgen3);
                }
                else if (word1.Equals("SysBio.EElimination"))
                {
                    EElimination tmpEElimination = new EElimination();
                    tmpEElimination.name = word2;
                    EElimination_list.Add(tmpEElimination);
                }
                else if (word1.Equals("SysBio.CEElimination"))
                {
                    CEElimination tmpCElimination = new CEElimination();
                    tmpCElimination.name = word2;
                    CEElimination_list.Add(tmpCElimination);
                }
                else if (word1.Equals("SysBio.ESource"))
                {
                    ESource tmpESource = new ESource();
                    tmpESource.name = word2;
                    ESource_list.Add(tmpESource);
                }


                //if we have connects, theres a difference in how we process it
                else if (word1.Equals("connect"))
                {
                    numOfConnections++;

                    //split params by "," - trim spaces
                    param1 = tmpword3.Split(',')[0];
                    param1 = param1.Trim();
                    param2 = tmpword3.Split(',')[1];
                    param2 = param2.Trim();

                    //find correct class - possible improvements!
                    //ESReaction
                    for (int j = 0; j < ESReaction_list.Count; j++)
                    {
                        //if the name of ESReaction matches any of the ESReactions, add the correct connection
                        if (((ESReaction)ESReaction_list[j]).name.Equals(param1.Split('.')[0]))
                        {
                            if (param1.Split('.')[1].Equals("outflow"))
                            {
                                ((ESReaction)ESReaction_list[j]).outflow = param2.Split('.')[0];
                            }
                            if (param1.Split('.')[1].Equals("inflow"))
                            {
                                ((ESReaction)ESReaction_list[j]).inflow = param2.Split('.')[0];
                            }
                            if (param1.Split('.')[1].Equals("enzyme"))
                            {
                                ((ESReaction)ESReaction_list[j]).enzyme = param2.Split('.')[0];
                            }
                        }
                        else if (((ESReaction)ESReaction_list[j]).name.Equals(param2.Split('.')[0]))
                        {
                            if (param2.Split('.')[1].Equals("outflow"))
                            {
                                ((ESReaction)ESReaction_list[j]).outflow = param1.Split('.')[0];
                            }
                            if (param2.Split('.')[1].Equals("inflow"))
                            {
                                ((ESReaction)ESReaction_list[j]).inflow = param1.Split('.')[0];
                            }
                            if (param2.Split('.')[1].Equals("enzyme"))
                            {
                                ((ESReaction)ESReaction_list[j]).enzyme = param1.Split('.')[0];
                            }
                        }
                    }

                    //EFormation
                    for (int j = 0; j < EFormation_list.Count; j++)
                    {
                        //if the name of ESReaction matches any of the ESReactions, add the correct connection
                        if (((EFormation_lin)EFormation_list[j]).name.Equals(param1.Split('.')[0]))
                        {
                            if (param1.Split('.')[1].Equals("outflow"))
                            {
                                ((EFormation_lin)EFormation_list[j]).outflow = param2.Split('.')[0];
                            }
                            if (param1.Split('.')[1].Equals("inflow"))
                            {
                                ((EFormation_lin)EFormation_list[j]).inflow = param2.Split('.')[0];
                            }
                        }
                        else if (((EFormation_lin)EFormation_list[j]).name.Equals(param2.Split('.')[0]))
                        {
                            if (param2.Split('.')[1].Equals("outflow"))
                            {
                                ((EFormation_lin)EFormation_list[j]).outflow = param1.Split('.')[0];
                            }
                            if (param2.Split('.')[1].Equals("inflow"))
                            {
                                ((EFormation_lin)EFormation_list[j]).inflow = param1.Split('.')[0];
                            }
                        }
                    }

                    //GeneExpression
                    for (int j = 0; j < GeneExpression_list.Count; j++)
                    {
                        //if the name of ESReaction matches any of the ESReactions, add the correct connection
                        if (((GenExpression_lin)GeneExpression_list[j]).name.Equals(param1.Split('.')[0]))
                        {
                            if (param1.Split('.')[1].Equals("mRNA"))
                            {
                                ((GenExpression_lin)GeneExpression_list[j]).mRNA = param2.Split('.')[0];
                            }

                        }
                        else if (((GenExpression_lin)GeneExpression_list[j]).name.Equals(param2.Split('.')[0]))
                        {
                            if (param2.Split('.')[1].Equals("mRNA"))
                            {
                                ((GenExpression_lin)GeneExpression_list[j]).mRNA = param1.Split('.')[0];
                            }
                        }
                    }

                    //GeneExpression_normal
                    for (int j = 0; j < GeneExpression_nor_list.Count; j++)
                    {
                        //if the name of ESReaction matches any of the ESReactions, add the correct connection
                        if (((GenExpression)GeneExpression_nor_list[j]).name.Equals(param1.Split('.')[0]))
                        {
                            if (param1.Split('.')[1].Equals("mRNA"))
                            {
                                ((GenExpression)GeneExpression_nor_list[j]).mRNA = param2.Split('.')[0];
                            }
                            if (param1.Split('.')[1].Equals("control"))
                            {
                                ((GenExpression)GeneExpression_nor_list[j]).control = param2.Split('.')[0];
                            }
                        }
                        else if (((GenExpression)GeneExpression_nor_list[j]).name.Equals(param2.Split('.')[0]))
                        {
                            if (param2.Split('.')[1].Equals("mRNA"))
                            {
                                ((GenExpression)GeneExpression_nor_list[j]).mRNA = param1.Split('.')[0];
                            }
                            if (param2.Split('.')[1].Equals("control"))
                            {
                                ((GenExpression)GeneExpression_nor_list[j]).control = param1.Split('.')[0];
                            }
                        }
                    }

                    //GeneExpression_positive
                    for (int j = 0; j < GeneExpression_pos_list.Count; j++)
                    {
                        //if the name of ESReaction matches any of the ESReactions, add the correct connection
                        if (((GenExpression_pos)GeneExpression_pos_list[j]).name.Equals(param1.Split('.')[0]))
                        {
                            if (param1.Split('.')[1].Equals("mRNA"))
                            {
                                ((GenExpression_pos)GeneExpression_pos_list[j]).mRNA = param2.Split('.')[0];
                            }
                            if (param1.Split('.')[1].Equals("control"))
                            {
                                ((GenExpression_pos)GeneExpression_pos_list[j]).control = param2.Split('.')[0];
                            }
                        }
                        else if (((GenExpression_pos)GeneExpression_pos_list[j]).name.Equals(param2.Split('.')[0]))
                        {
                            if (param2.Split('.')[1].Equals("mRNA"))
                            {
                                ((GenExpression_pos)GeneExpression_pos_list[j]).mRNA = param1.Split('.')[0];
                            }
                            if (param2.Split('.')[1].Equals("control"))
                            {
                                ((GenExpression_pos)GeneExpression_pos_list[j]).control = param1.Split('.')[0];
                            }
                        }
                    }

                    //GeneExpression_negative
                    for (int j = 0; j < GeneExpression_neg_list.Count; j++)
                    {
                        //if the name of ESReaction matches any of the ESReactions, add the correct connection
                        if (((GenExpression_neg)GeneExpression_neg_list[j]).name.Equals(param1.Split('.')[0]))
                        {
                            if (param1.Split('.')[1].Equals("mRNA"))
                            {
                                ((GenExpression_neg)GeneExpression_neg_list[j]).mRNA = param2.Split('.')[0];
                            }
                            if (param1.Split('.')[1].Equals("control"))
                            {
                                ((GenExpression_neg)GeneExpression_neg_list[j]).control = param2.Split('.')[0];
                            }
                        }
                        else if (((GenExpression_neg)GeneExpression_neg_list[j]).name.Equals(param2.Split('.')[0]))
                        {
                            if (param2.Split('.')[1].Equals("mRNA"))
                            {
                                ((GenExpression_neg)GeneExpression_neg_list[j]).mRNA = param1.Split('.')[0];
                            }
                            if (param2.Split('.')[1].Equals("control"))
                            {
                                ((GenExpression_neg)GeneExpression_neg_list[j]).control = param1.Split('.')[0];
                            }
                        }
                    }

                    //Source
                    for (int j = 0; j < GeneExpression_list.Count; j++)
                    {
                        //if the name of ESReaction matches any of the ESReactions, add the correct 
                        if (Source_list.Count != 0)
                        {
                            if (((Source)Source_list[j]).name.Equals(param1.Split('.')[0]))
                            {
                                if (param1.Split('.')[1].Equals("out"))
                                {
                                    ((Source)Source_list[j]).outflow = param2.Split('.')[0];
                                }
                            }
                            else if (((Source)Source_list[j]).name.Equals(param2.Split('.')[0]))
                            {
                                if (param2.Split('.')[1].Equals("out"))
                                {
                                    ((Source)Source_list[j]).outflow = param1.Split('.')[0];
                                }
                            }
                        }
                    }

                    //ESource
                    for (int j = 0; j < ESource_list.Count; j++)
                    {
                        //if the name of ESReaction matches any of the ESReactions, add the correct 
                        if (ESource_list.Count != 0)
                        {
                            if (((ESource)ESource_list[j]).name.Equals(param1.Split('.')[0]))
                            {
                                if (param1.Split('.')[1].Equals("outflow"))
                                {
                                    ((ESource)ESource_list[j]).outflow = param2.Split('.')[0];
                                }
                                if (param1.Split('.')[1].Equals("enzyme"))
                                {
                                    ((ESource)ESource_list[j]).enzyme = param2.Split('.')[0];
                                }
                            }
                            else if (((ESource)ESource_list[j]).name.Equals(param2.Split('.')[0]))
                            {
                                if (param2.Split('.')[1].Equals("outflow"))
                                {
                                    ((ESource)ESource_list[j]).outflow = param1.Split('.')[0];
                                }
                                if (param2.Split('.')[1].Equals("enzyme"))
                                {
                                    ((ESource)ESource_list[j]).enzyme = param1.Split('.')[0];
                                }
                            }
                        }
                    }

                    //Elimination
                    for (int j = 0; j < Elimination_list.Count; j++)
                    {
                        //if the name of ESReaction matches any of the ESReactions, add the correct connection
                        if (((Elimination)Elimination_list[j]).name.Equals(param1.Split('.')[0]))
                        {
                            if (param1.Split('.')[1].Equals("inflow"))
                            {
                                ((Elimination)Elimination_list[j]).inflow = param2.Split('.')[0];
                            }
                        }
                        else if (((Elimination)Elimination_list[j]).name.Equals(param2.Split('.')[0]))
                        {
                            if (param2.Split('.')[1].Equals("inflow"))
                            {
                                ((Elimination)Elimination_list[j]).inflow = param1.Split('.')[0];
                            }
                        }
                    }

                    //EElimination
                    for (int j = 0; j < EElimination_list.Count; j++)
                    {
                        //if the name of ESReaction matches any of the ESReactions, add the correct connection
                        if (((EElimination)EElimination_list[j]).name.Equals(param1.Split('.')[0]))
                        {
                            if (param1.Split('.')[1].Equals("inflow"))
                            {
                                ((EElimination)EElimination_list[j]).inflow = param2.Split('.')[0];
                            }
                            if (param1.Split('.')[1].Equals("enzyme"))
                            {
                                ((EElimination)EElimination_list[j]).enzyme = param2.Split('.')[0];
                            }
                        }
                        else if (((EElimination)EElimination_list[j]).name.Equals(param2.Split('.')[0]))
                        {
                            if (param2.Split('.')[1].Equals("inflow"))
                            {
                                ((EElimination)EElimination_list[j]).inflow = param1.Split('.')[0];
                            }
                            if (param2.Split('.')[1].Equals("enzyme"))
                            {
                                ((EElimination)EElimination_list[j]).enzyme = param1.Split('.')[0];
                            }
                        }
                    }

                    //CEElimination
                    for (int j = 0; j < CEElimination_list.Count; j++)
                    {
                        //if the name of ESReaction matches any of the ESReactions, add the correct connection
                        if (((CEElimination)CEElimination_list[j]).name.Equals(param1.Split('.')[0]))
                        {
                            if (param1.Split('.')[1].Equals("inflow"))
                            {
                                ((CEElimination)CEElimination_list[j]).inflow = param2.Split('.')[0];
                            }
                            if (param1.Split('.')[1].Equals("control"))
                            {
                                ((CEElimination)EElimination_list[j]).control = param2.Split('.')[0];
                            }
                        }
                        else if (((CEElimination)CEElimination_list[j]).name.Equals(param2.Split('.')[0]))
                        {
                            if (param2.Split('.')[1].Equals("inflow"))
                            {
                                ((CEElimination)CEElimination_list[j]).inflow = param1.Split('.')[0];
                            }
                            if (param2.Split('.')[1].Equals("control"))
                            {
                                ((CEElimination)CEElimination_list[j]).control = param1.Split('.')[0];
                            }
                        }
                    }
                }
            }

            //statistics
            model.numOfEFormation = EFormation_list.Count;
            model.numOfEnzyme = Enzyme_list.Count;
            model.numOfESReaction = ESReaction_list.Count;
            model.numOfGenExpression = GeneExpression_list.Count;
            model.numOfMetabolite = Metabolite_list.Count;
            model.numOfmRNA = mRNA_list.Count;
            model.numOfSource = Source_list.Count;
            model.numOfElimination = Elimination_list.Count;
            model.numOfGenExpression_pos = GeneExpression_pos_list.Count;
            model.numOfGenExpression_neg = GeneExpression_neg_list.Count;
            model.numOfGenExpression_nor = GeneExpression_nor_list.Count;
            model.numOfEElimination = EElimination_list.Count;
            model.numOfCEElimination = CEElimination_list.Count;
            model.numOfESource = ESource_list.Count;
            model.numOfProtein = Protein_list.Count;
            

            /*-----------------------------------------------------------------START BUILDING SBML FILE-----------------------------------------------------------*/

            // list of elements for closing brackets and TAB spaces 
            ArrayList listOfElements = new ArrayList();

            //SBML strings that are always present
            String SBML = "";
            SBML += "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n\r\n";
            SBML += "<sbml xmlns=\"http://www.sbml.org/sbml/level2/version3\" level=\"2\" version=\"3\">\r\n";
            listOfElements.Add("sbml");


            //model ID, name
            SBML += addTabs(listOfElements);
            SBML += "<model id=\"" + model.name + "\" name=\"model_" + model.name+"\">\r\n";
            listOfElements.Add("model");


            //compartment
            SBML += addTabs(listOfElements);
            SBML += "<listOfCompartments>\r\n";
            listOfElements.Add("listOfCompartments");
            SBML += addTabs(listOfElements);
            SBML += "<compartment id=\"" + compartment_name + "\" name=\"" + compartment_name + "\" spatialDimensions=\"3\" size=\"1\" constant=\"true\"/>\r\n";
            SBML += addTabsMinusOne(listOfElements);
            SBML += "</" + listOfElements[listOfElements.Count-1] + ">\r\n\r\n";
            listOfElements.RemoveAt(listOfElements.Count-1);

            //species
            SBML += addTabs(listOfElements);
            SBML += "<listOfSpecies>\r\n";
            listOfElements.Add("listOfSpecies");
            //mRNA
            for(int i = 0; i < mRNA_list.Count; i++)
            {
                SBML += addTabs(listOfElements);
                SBML += "<species id=\"" + ((mRNA)mRNA_list[i]).name + "\" name=\"" + ((mRNA)mRNA_list[i]).name + "\" compartment=\""+ compartment_name +"\"/>\r\n";
            }
            //Protein
            for (int i = 0; i < Protein_list.Count; i++)
            {
                SBML += addTabs(listOfElements);
                SBML += "<species id=\"" + ((Protein)Protein_list[i]).name + "\" name=\"" + ((Protein)Protein_list[i]).name + "\" compartment=\"" + compartment_name + "\"/>\r\n";
            }
            //CMass
            for (int i = 0; i < CMass_list.Count; i++)
            {
                SBML += addTabs(listOfElements);
                SBML += "<species id=\"" + ((CMass)CMass_list[i]).name + "\" name=\"" + ((CMass)CMass_list[i]).name + "\" compartment=\"" + compartment_name + "\"/>\r\n";
            }
            //Metabolite
            for (int i = 0; i < Metabolite_list.Count; i++)
            {
                SBML += addTabs(listOfElements);
                SBML += "<species id=\"" + ((Metabolite)Metabolite_list[i]).name + "\" name=\"" + ((Metabolite)Metabolite_list[i]).name + "\" compartment=\"" + compartment_name + "\"/>\r\n";
            }
            //Enzyme
            for (int i = 0; i < Enzyme_list.Count; i++)
            {
                SBML += addTabs(listOfElements);
                SBML += "<species id=\"" + ((Enzyme)Enzyme_list[i]).name + "\" name=\"" + ((Enzyme)Enzyme_list[i]).name + "\" compartment=\"" + compartment_name + "\"/>\r\n";
            }
            SBML += addTabsMinusOne(listOfElements);
            SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n\r\n";
            listOfElements.RemoveAt(listOfElements.Count - 1);


            //reactions
            SBML += addTabs(listOfElements);
            SBML += "<listOfReactions>\r\n";
            listOfElements.Add("listOfReactions");
            //Source
            for(int i = 0; i < Source_list.Count; i++)
            {
                SBML += addTabs(listOfElements);
                SBML += "<reaction id=\"" + ((Source)Source_list[i]).name + "\" name=\"" + ((Source)Source_list[i]).name + "\" reversible=\"" + reversibility_Source.ToString().ToLowerInvariant() + "\">\r\n";
                listOfElements.Add("reaction");
                //no reactants
                SBML += addTabs(listOfElements);
                SBML += "<listOfReactants>\r\n" + addTabs(listOfElements) + "</listOfReactants>\r\n";
                SBML += addTabs(listOfElements);
                SBML += "<listOfProducts>\r\n";
                listOfElements.Add("listOfProducts");
                SBML += addTabs(listOfElements);
                SBML += "<speciesReference species=\"" + ((Source)Source_list[i]).outflow + "\"/>\r\n";
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
            }
            //ESource
            for (int i = 0; i < ESource_list.Count; i++)
            {
                SBML += addTabs(listOfElements);
                SBML += "<reaction id=\"" + ((ESource)ESource_list[i]).name + "\" name=\"" + ((ESource)ESource_list[i]).name + "\" reversible=\"" + reversibility_EFormation.ToString().ToLowerInvariant() + "\">\r\n";
                listOfElements.Add("reaction");
                SBML += addTabs(listOfElements);
                SBML += "<listOfReactants>\r\n";
                listOfElements.Add("listOfReactants");
                SBML += addTabs(listOfElements);
                SBML += "<speciesReference species=\"" + ((ESource)ESource_list[i]).enzyme + "\"/>\r\n";
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
                SBML += addTabs(listOfElements);
                SBML += "<listOfProducts>\r\n";
                listOfElements.Add("listOfProducts");
                SBML += addTabs(listOfElements);
                SBML += "<speciesReference species=\"" + ((ESource)ESource_list[i]).outflow + "\"/>\r\n";
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
            }

            //GeneExpression
            for (int i = 0; i < GeneExpression_list.Count; i++)
            {
                SBML += addTabs(listOfElements);
                SBML += "<reaction id=\"" + ((GenExpression_lin)GeneExpression_list[i]).name + "\" name=\"" + ((GenExpression_lin)GeneExpression_list[i]).name + "\" reversible=\"" + reversibility_GenExp.ToString().ToLowerInvariant() + "\">\r\n";
                listOfElements.Add("reaction");
                //no reactants
                SBML += addTabs(listOfElements);
                SBML += "<listOfReactants>\r\n" + addTabs(listOfElements) + "</listOfReactants>\r\n";
                SBML += addTabs(listOfElements);
                SBML += "<listOfProducts>\r\n";
                listOfElements.Add("listOfProducts");
                SBML += addTabs(listOfElements);
                SBML += "<speciesReference species=\"" + ((GenExpression_lin)GeneExpression_list[i]).mRNA + "\"/>\r\n";
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
            }
            //GeneExpression_neg
            for (int i = 0; i < GeneExpression_neg_list.Count; i++)
            {
                SBML += addTabs(listOfElements);
                SBML += "<reaction id=\"" + ((GenExpression_neg)GeneExpression_neg_list[i]).name + "\" name=\"" + ((GenExpression_neg)GeneExpression_neg_list[i]).name + "\" reversible=\"" + reversibility_EFormation.ToString().ToLowerInvariant() + "\">\r\n";
                listOfElements.Add("reaction");
                SBML += addTabs(listOfElements);
                SBML += "<listOfReactants>\r\n";
                listOfElements.Add("listOfReactants");
                SBML += addTabs(listOfElements);
                SBML += "<speciesReference species=\"" + ((GenExpression_neg)GeneExpression_neg_list[i]).control + "\"/>\r\n";
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
                SBML += addTabs(listOfElements);
                SBML += "<listOfProducts>\r\n";
                listOfElements.Add("listOfProducts");
                SBML += addTabs(listOfElements);
                SBML += "<speciesReference species=\"" + ((GenExpression_neg)GeneExpression_neg_list[i]).mRNA + "\"/>\r\n";
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
            }
            //GeneExpression_nor
            for (int i = 0; i < GeneExpression_nor_list.Count; i++)
            {
                SBML += addTabs(listOfElements);
                SBML += "<reaction id=\"" + ((GenExpression)GeneExpression_nor_list[i]).name + "\" name=\"" + ((GenExpression)GeneExpression_nor_list[i]).name + "\" reversible=\"" + reversibility_EFormation.ToString().ToLowerInvariant() + "\">\r\n";
                listOfElements.Add("reaction");
                SBML += addTabs(listOfElements);
                SBML += "<listOfReactants>\r\n";
                listOfElements.Add("listOfReactants");
                SBML += addTabs(listOfElements);
                SBML += "<speciesReference species=\"" + ((GenExpression)GeneExpression_nor_list[i]).control + "\"/>\r\n";
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
                SBML += addTabs(listOfElements);
                SBML += "<listOfProducts>\r\n";
                listOfElements.Add("listOfProducts");
                SBML += addTabs(listOfElements);
                SBML += "<speciesReference species=\"" + ((GenExpression)GeneExpression_nor_list[i]).mRNA + "\"/>\r\n";
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
            }


            //EFormation
            for (int i = 0; i < EFormation_list.Count; i++)
            {
                SBML += addTabs(listOfElements);
                SBML += "<reaction id=\"" + ((EFormation_lin)EFormation_list[i]).name + "\" name=\"" + ((EFormation_lin)EFormation_list[i]).name + "\" reversible=\"" + reversibility_EFormation.ToString().ToLowerInvariant() + "\">\r\n";
                listOfElements.Add("reaction");
                SBML += addTabs(listOfElements);
                SBML += "<listOfReactants>\r\n";
                listOfElements.Add("listOfReactants");
                SBML += addTabs(listOfElements);
                SBML += "<speciesReference species=\"" + ((EFormation_lin)EFormation_list[i]).inflow + "\"/>\r\n";
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
                SBML += addTabs(listOfElements);
                SBML += "<listOfProducts>\r\n";
                listOfElements.Add("listOfProducts");
                SBML += addTabs(listOfElements);
                SBML += "<speciesReference species=\"" + ((EFormation_lin)EFormation_list[i]).outflow + "\"/>\r\n";
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
            }
            //ESReaction
            for (int i = 0; i < ESReaction_list.Count; i++)
            {
                SBML += addTabs(listOfElements);
                SBML += "<reaction id=\"" + ((ESReaction)ESReaction_list[i]).name + "\" name=\"" + ((ESReaction)ESReaction_list[i]).name + "\" reversible=\"" + reversibility_ESReaction.ToString().ToLowerInvariant() + "\">\r\n";
                listOfElements.Add("reaction");
                SBML += addTabs(listOfElements);
                SBML += "<listOfReactants>\r\n";
                listOfElements.Add("listOfReactants");
                SBML += addTabs(listOfElements);
                SBML += "<speciesReference species=\"" + ((ESReaction)ESReaction_list[i]).inflow + "\"/>\r\n";
                SBML += addTabs(listOfElements);
                SBML += "<speciesReference species=\"" + ((ESReaction)ESReaction_list[i]).enzyme + "\"/>\r\n";
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
                SBML += addTabs(listOfElements);
                SBML += "<listOfProducts>\r\n";
                listOfElements.Add("listOfProducts");
                SBML += addTabs(listOfElements);
                SBML += "<speciesReference species=\"" + ((ESReaction)ESReaction_list[i]).outflow + "\"/>\r\n";
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
            }
            //Elimination
            for (int i = 0; i < Elimination_list.Count; i++)
            {
                SBML += addTabs(listOfElements);
                SBML += "<reaction id=\"" + ((Elimination)Elimination_list[i]).name + "\" name=\"" + ((Elimination)Elimination_list[i]).name + "\" reversible=\"" + reversibility_Elimination.ToString().ToLowerInvariant() + "\">\r\n";
                listOfElements.Add("reaction");
                SBML += addTabs(listOfElements);
                SBML += "<listOfReactants>\r\n";
                listOfElements.Add("listOfReactants");
                SBML += addTabs(listOfElements);
                SBML += "<speciesReference species=\"" + ((Elimination)Elimination_list[i]).inflow + "\"/>\r\n";
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
                SBML += addTabs(listOfElements);
                SBML += "<listOfProducts>\r\n";
                listOfElements.Add("listOfProducts");
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
            }
            //EElimination
            for (int i = 0; i < EElimination_list.Count; i++)
            {
                SBML += addTabs(listOfElements);
                SBML += "<reaction id=\"" + ((EElimination)EElimination_list[i]).name + "\" name=\"" + ((EElimination)EElimination_list[i]).name + "\" reversible=\"" + reversibility_Elimination.ToString().ToLowerInvariant() + "\">\r\n";
                listOfElements.Add("reaction");
                SBML += addTabs(listOfElements);
                SBML += "<listOfReactants>\r\n";
                listOfElements.Add("listOfReactants");
                SBML += addTabs(listOfElements);
                SBML += "<speciesReference species=\"" + ((EElimination)EElimination_list[i]).inflow + "\"/>\r\n";
                SBML += addTabs(listOfElements);
                SBML += "<speciesReference species=\"" + ((EElimination)EElimination_list[i]).enzyme + "\"/>\r\n";
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
                SBML += addTabs(listOfElements);
                SBML += "<listOfProducts>\r\n";
                listOfElements.Add("listOfProducts");
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
            }

            //CEElimination
            for (int i = 0; i < CEElimination_list.Count; i++)
            {
                SBML += addTabs(listOfElements);
                SBML += "<reaction id=\"" + ((CEElimination)CEElimination_list[i]).name + "\" name=\"" + ((CEElimination)CEElimination_list[i]).name + "\" reversible=\"" + reversibility_Elimination.ToString().ToLowerInvariant() + "\">\r\n";
                listOfElements.Add("reaction");
                SBML += addTabs(listOfElements);
                SBML += "<listOfReactants>\r\n";
                listOfElements.Add("listOfReactants");
                SBML += addTabs(listOfElements);
                SBML += "<speciesReference species=\"" + ((CEElimination)CEElimination_list[i]).inflow + "\"/>\r\n";
                SBML += addTabs(listOfElements);
                SBML += "<speciesReference species=\"" + ((CEElimination)CEElimination_list[i]).control + "\"/>\r\n";
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
                SBML += addTabs(listOfElements);
                SBML += "<listOfProducts>\r\n";
                listOfElements.Add("listOfProducts");
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
                SBML += addTabsMinusOne(listOfElements);
                SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n\r\n";
                listOfElements.RemoveAt(listOfElements.Count - 1);
            }




            //end reactions
            SBML += addTabsMinusOne(listOfElements);
            SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
            listOfElements.RemoveAt(listOfElements.Count - 1);

            //end model
            SBML += addTabsMinusOne(listOfElements);
            SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
            listOfElements.RemoveAt(listOfElements.Count - 1);

            //end sbml
            SBML += addTabsMinusOne(listOfElements);
            SBML += "</" + listOfElements[listOfElements.Count - 1] + ">\r\n";
            listOfElements.RemoveAt(listOfElements.Count - 1);



            //debug print file
            /* for (int i = 0; i < datoteka.Count; i++)
             {
                 tb_statistics.Text += datoteka[i] + "\r\n";
             }*/

            //debug ESReactions
            //tb_statistics.Text += ((ESReaction)ESReaction_list[0]).name + " " + ((ESReaction)ESReaction_list[0]).inflow + " " + ((ESReaction)ESReaction_list[0]).outflow + " " + ((ESReaction)ESReaction_list[0]).enzyme + "\r\n";
            //tb_statistics.Text += ((EFormation_lin)EFormation_list[0]).name + " " + ((EFormation_lin)EFormation_list[0]).inflow + " " + ((EFormation_lin)EFormation_list[0]).outflow + "\r\n";


            //print all stats
            tb_statistics.Text += "Model name: " + model.name + "\r\n";
            tb_statistics.Text += "Total # of EFormations: " + model.numOfEFormation + "\r\n";
            tb_statistics.Text += "Total # of Enzymes: " + model.numOfEnzyme + "\r\n";
            tb_statistics.Text += "Total # of ESReactions: " + model.numOfESReaction + "\r\n";
            tb_statistics.Text += "Total # of GeneExpressions_lin: " + model.numOfGenExpression + "\r\n";
            tb_statistics.Text += "Total # of GeneExpressions_pos: " + model.numOfGenExpression_pos + "\r\n";
            tb_statistics.Text += "Total # of GeneExpressions_neg: " + model.numOfGenExpression_neg + "\r\n";
            tb_statistics.Text += "Total # of GeneExpressions_nor: " + model.numOfGenExpression_nor + "\r\n";
            tb_statistics.Text += "Total # of Metabolites: " + model.numOfMetabolite + "\r\n";
            tb_statistics.Text += "Total # of mRNAs: " + model.numOfmRNA + "\r\n";
            tb_statistics.Text += "Total # of Proteins: " + model.numOfProtein + "\r\n";
            tb_statistics.Text += "Total # of Sources: " + model.numOfSource + "\r\n";
            tb_statistics.Text += "Total # of ESources: " + model.numOfESource + "\r\n";
            tb_statistics.Text += "Total # of Eliminations: " + model.numOfElimination + "\r\n";
            tb_statistics.Text += "Total # of EEliminations: " + model.numOfEElimination + "\r\n";
            tb_statistics.Text += "Total # of CEliminations: " + model.numOfCEElimination + "\r\n";
            tb_statistics.Text += "Total # of Connections: " + numOfConnections + "\r\n";
            
            tb_statistics.Text += "-----------------------------------\r\n";
            //tb_statistics.Text += SBML;
            //tb_statistics.Text += ((Elimination)Elimination_list[0]).inflow;
            
            return SBML;

        }

        private static String addTabs(ArrayList list)
        {
            String s = "";
            for (int i = 0; i < list.Count; i++)
            {
                s += "\t";
            }

            return s;
        }

        private static String addTabsMinusOne(ArrayList list)
        {
            String s = "";
            for (int i = 0; i < list.Count-1; i++)
            {
                s += "\t";
            }

            return s;
        }

        
    }

    public class Model_statistics
    {
        public String name { get; set; }
        public int numOfSource { get; set; }
        public int numOfmRNA { get; set; }
        public int numOfMetabolite { get; set; }
        public int numOfEnzyme { get; set; }
        public int numOfESReaction { get; set; }
        public int numOfEFormation { get; set; }
        public int numOfGenExpression { get; set; }
        public int numOfElimination { get; set; }
        public int numOfGenExpression_neg { get; set; }
        public int numOfGenExpression_pos { get; set; }
        public int numOfGenExpression_nor { get; set; }
        public int numOfEElimination { get; set; }
        public int numOfCEElimination { get; set; }
        public int numOfESource { get; set; }
        public int numOfProtein { get; set; }

        public Model_statistics()
        {
            name = "";
            numOfSource = 0;
            numOfmRNA = 0;
            numOfMetabolite = 0;
            numOfEnzyme = 0;
            numOfESReaction = 0;
            numOfEFormation = 0;
            numOfGenExpression = 0;
            numOfElimination = 0;
            numOfGenExpression_neg = 0;
            numOfGenExpression_pos = 0;
            numOfGenExpression_nor = 0;
            numOfEElimination = 0;
            numOfCEElimination = 0;
            numOfESource = 0;
            numOfProtein = 0;
        }
        
    }

   

    public class Source
    {
        public String name { get; set; }
        public String outflow { get; set; }
    }

    public class ESource
    {
        public String name { get; set; }
        public String outflow { get; set; }
        public String enzyme { get; set; }
    }

    public class EElimination
    {
        public String name { get; set; }
        public String enzyme { get; set; }
        public String inflow { get; set; }
    }

    public class CEElimination
    {
        public String name { get; set; }
        public String control { get; set; }
        public String inflow { get; set; }
    }

    public class GenExpression_pos
    {
        public String name { get; set; }
        public String control { get; set; }
        public String mRNA { get; set; }
    }

    public class GenExpression_neg
    {
        public String name { get; set; }
        public String control { get; set; }
        public String mRNA { get; set; }
    }


    public class GenExpression_lin
    {
        public String name { get; set; }
        public String mRNA { get; set; }
    }

    public class GenExpression
    {
        public String name { get; set; }
        public String mRNA { get; set; }
        public String control { get; set; }
    }
        
    public class ESReaction
    {
        public String name { get; set; }
        public String inflow { get; set; }
        public String outflow { get; set; }
        public String enzyme { get; set; }

    }

    public class Elimination
    {
        public String name { get; set; }
        public String inflow { get; set; }
    }

    public class EFormation_lin
    {
        public String name { get; set; }
        public String inflow { get; set; }
        public String outflow { get; set; }
    }

    public class Metabolite
    {
        public String name { get; set; }
    }

    public class mRNA
    {
        public String name { get; set; }
    }

    public class Enzyme
    {
        public String name { get; set; }
    }

    public class Protein
    {
        public String name { get; set; }
    }

    public class CMass
    {
        public String name { get; set; }
    }

}
