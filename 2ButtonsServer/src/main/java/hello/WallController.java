package hello;

import hello.dbCalls.Login;
import hello.dbCalls.QuestionsList;
import hello.dbCalls.UserPage;
import org.json.JSONArray;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.sql.ResultSet;
import java.sql.SQLException;

@RestController
public class WallController { //To get user's posts
    @RequestMapping(value = "/getPosts") //Можно в скобках дописать method = RequestMethod.GET чтобы знать, к какому методу мы обращаемся. По какому URL обращаемся к методу и каким http методом (GET, POST) А так обращается по всем
    public String getPosts(@RequestParam(value = "id", required = true) int id, @RequestParam(value = "userId", required = true) int userId, @RequestParam(value = "amount", required = false, defaultValue = "100") int amount) {
        MsSqlConnection connection = new MsSqlConnection();

        ResultSet posts = UserPage.getPosts(connection.con, id, userId, amount);

        /*try {
            posts.beforeFirst();;
        } catch (SQLException e) {
            e.printStackTrace();
        }*/

        JSONArray arr = new JSONArray();
        try {
            try {
                arr = ResultSetConverter.convert(posts);
            } catch (org.json.JSONException e) {
                e.printStackTrace();
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }

        System.out.println(arr.toString());

        return arr.toString();
    }

    @RequestMapping(value = "/getUserAskedQuestions") //Можно в скобках дописать method = RequestMethod.GET чтобы знать, к какому методу мы обращаемся. По какому URL обращаемся к методу и каким http методом (GET, POST) А так обращается по всем
    public String getUserAskedQuestions(@RequestParam(value = "id", required = true) int id, @RequestParam(value = "userId", required = true) int userId, @RequestParam(value = "amount", required = false, defaultValue = "100") int amount) {
        MsSqlConnection connection = new MsSqlConnection();

        ResultSet posts = QuestionsList.getUserAskedQuestions(connection.con, id, userId, amount, false);

        JSONArray arr = new JSONArray();
        try {
            try {
                arr = ResultSetConverter.convert(posts);
            } catch (org.json.JSONException e) {
                e.printStackTrace();
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }

        System.out.println(arr.toString());

        return arr.toString();
    }

    @RequestMapping(value = "/getUserAnsweredQuestions") //Можно в скобках дописать method = RequestMethod.GET чтобы знать, к какому методу мы обращаемся. По какому URL обращаемся к методу и каким http методом (GET, POST) А так обращается по всем
    public String getUserAnsweredQuestions(@RequestParam(value = "id", required = true) int id, @RequestParam(value = "userId", required = true) int userId, @RequestParam(value = "amount", required = false, defaultValue = "100") int amount) {
        MsSqlConnection connection = new MsSqlConnection();

        ResultSet posts = QuestionsList.getUserAnsweredQuestions(connection.con, id, userId, amount, true);

        JSONArray arr = new JSONArray();
        try {
            try {
                arr = ResultSetConverter.convert(posts);
            } catch (org.json.JSONException e) {
                e.printStackTrace();
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }

        return arr.toString();
    }

    @RequestMapping(value = "/getUserFavoriteQuestions") //Можно в скобках дописать method = RequestMethod.GET чтобы знать, к какому методу мы обращаемся. По какому URL обращаемся к методу и каким http методом (GET, POST) А так обращается по всем
    public String getUserFavoriteQuestions(@RequestParam(value = "id", required = true) int id, @RequestParam(value = "userId", required = true) int userId, @RequestParam(value = "amount", required = false, defaultValue = "100") int amount) {
        MsSqlConnection connection = new MsSqlConnection();

        ResultSet posts = QuestionsList.getUserFavoriteQuestions(connection.con, id, userId, amount, false);

        JSONArray arr = new JSONArray();
        try {
            try {
                arr = ResultSetConverter.convert(posts);
            } catch (org.json.JSONException e) {
                e.printStackTrace();
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }

        return arr.toString();
    }

    @RequestMapping(value = "/getUserCommentedQuestions") //Можно в скобках дописать method = RequestMethod.GET чтобы знать, к какому методу мы обращаемся. По какому URL обращаемся к методу и каким http методом (GET, POST) А так обращается по всем
    public String getUserCommentedQuestions(@RequestParam(value = "id", required = true) int id, @RequestParam(value = "userId", required = true) int userId, @RequestParam(value = "amount", required = false, defaultValue = "100") int amount) {
        MsSqlConnection connection = new MsSqlConnection();

        ResultSet posts = QuestionsList.getUserCommentedQuestions(connection.con, id, userId, amount);

        JSONArray arr = new JSONArray();
        try {
            try {
                arr = ResultSetConverter.convert(posts);
            } catch (org.json.JSONException e) {
                e.printStackTrace();
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }

        return arr.toString();
    }
}
